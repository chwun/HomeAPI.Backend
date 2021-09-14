using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.TimeSeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TimeSeriesController : ControllerBase
	{
		private readonly IInfluxDBProvider influxDbProvider;
		private readonly PreconfiguredTimeSeries preconfiguredTimeSeries;

		public TimeSeriesController(IInfluxDBProvider influxDbProvider, IOptionsMonitor<PreconfiguredTimeSeries> optionsMonitor)
		{
			this.influxDbProvider = influxDbProvider;
			preconfiguredTimeSeries = optionsMonitor.CurrentValue;
		}

		[HttpGet()]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<TimeSeriesResult>> GetTimeSeries(
			[FromQuery] string measurementName,
			[FromQuery] string location,
			[FromQuery] TimeSeriesRange range)
		{
			if (measurementName is null || location is null)
			{
				return BadRequest();
			}

			TimeSeriesRequest request = new()
			{
				MeasurementName = measurementName,
				Tags = new()
				{
					["location"] = location
				},
				ValueType = TimeSeriesValueType.Float,
				Range = range
			};

			var timeSeriesResponse = await influxDbProvider.GetTimeSeriesAsync(request, $"{measurementName} {location}");
			var status = timeSeriesResponse.Status;

			if (status == TimeSeriesResponseStatus.BadRequest)
			{
				return BadRequest();
			}
			else if (status == TimeSeriesResponseStatus.InternalError)
			{
				return NotFound();
			}

			return Ok(timeSeriesResponse.TimeSeriesResult);
		}

		[HttpGet("preconfigured")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<TimeSeriesResult>>> GetPreconfiguredTimeSeries([FromQuery] TimeSeriesRange range)
		{
			List<TimeSeriesResult> results = new();

			foreach (var element in preconfiguredTimeSeries.Elements)
			{
				if (element.MeasurementName is null || element.MeasurementLocation is null)
				{
					return BadRequest();
				}

				TimeSeriesRequest request = new()
				{
					MeasurementName = element.MeasurementName,
					Tags = new()
					{
						["location"] = element.MeasurementLocation
					},
					ValueType = TimeSeriesValueType.Float,
					Range = range
				};

				var timeSeriesResponse = await influxDbProvider.GetTimeSeriesAsync(request, element.DisplayName);
				var status = timeSeriesResponse.Status;

				if (status == TimeSeriesResponseStatus.BadRequest)
				{
					return BadRequest();
				}
				else if (status == TimeSeriesResponseStatus.InternalError)
				{
					return NotFound();
				}

				results.Add(timeSeriesResponse.TimeSeriesResult);
			}

			return Ok(results);
		}
	}
}
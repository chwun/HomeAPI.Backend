using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Providers.TimeSeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TimeSeriesController : ControllerBase
	{
		private readonly IInfluxDBProvider influxDbProvider;

		public TimeSeriesController(IInfluxDBProvider influxDbProvider)
		{
			this.influxDbProvider = influxDbProvider;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<DataPoint>>> GetTimeSeries([FromBody] TimeSeriesRequest request)
		{
			if (request == null)
			{
				return BadRequest();
			}

			var timeSeriesResponse = await influxDbProvider.GetTimeSeriesAsync(request);
			var status = timeSeriesResponse.Status;

			if (status == TimeSeriesResponseStatus.BadRequest)
			{
				return BadRequest();
			}
			else if (status == TimeSeriesResponseStatus.InternalError)
			{
				return NotFound();
			}

			return Ok(timeSeriesResponse.DataPoints);
		}
	}
}
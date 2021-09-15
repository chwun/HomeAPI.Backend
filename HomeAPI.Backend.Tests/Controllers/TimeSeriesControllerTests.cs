using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.TimeSeries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class TimeSeriesControllerTests
	{
		#region default data

		PreconfiguredTimeSeries preconfiguredTimeSeries = new()
		{
			Elements = new()
			{
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = "temperature",
					MeasurementLocation = "inside"
				}
			}
		};

		#endregion

		#region GetTimeSeries

		[Fact]
		public async Task GetTimeSeries_InternalError()
		{
			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.InternalError,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = null
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", "Room1", TimeSeriesRange.OneDay);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetTimeSeries_BadRequest()
		{
			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.BadRequest,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = null
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", "Room1", TimeSeriesRange.OneDay);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetTimeSeries_MeasurementNameNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries(null, "inside", TimeSeriesRange.OneMonth);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetTimeSeries_MeasurementLocationNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", null, TimeSeriesRange.OneMonth);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetTimeSeries_Success()
		{
			var dataPoints = new List<DataPoint>()
			{
				new DataPoint<float>(new DateTime(2020, 10, 30), 3.14f),
				new DataPoint<float>(new DateTime(2020, 10, 31), 3.14f)
			};
			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = dataPoints
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", "Room1", TimeSeriesRange.OneDay);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObj = Assert.IsType<TimeSeriesResult>(okResult.Value);
			Assert.Equal(dataPoints, resultObj.DataPoints);
		}

		#endregion
	}
}
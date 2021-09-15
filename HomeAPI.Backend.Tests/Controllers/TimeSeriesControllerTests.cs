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

		PreconfiguredTimeSeries preconfiguredTimeSeries1 = new()
		{
			Elements = new()
			{
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = "temperature",
					MeasurementLocation = "Room1"
				}
			}
		};

		PreconfiguredTimeSeries preconfiguredTimeSeries2 = new()
		{
			Elements = new()
			{
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = "temperature",
					MeasurementLocation = "Room1"
				},
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = "temperature",
					MeasurementLocation = "Room2"
				}
			}
		};

		PreconfiguredTimeSeries preconfiguredTimeSeries3 = new()
		{
			Elements = new()
			{
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = null,
					MeasurementLocation = "Room1"
				}
			}
		};

		PreconfiguredTimeSeries preconfiguredTimeSeries4 = new()
		{
			Elements = new()
			{
				new()
				{
					DisplayName = "xyz1",
					MeasurementName = "temperature",
					MeasurementLocation = null
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
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries1);
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
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries1);
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

			await influxProvider.DidNotReceive().GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>());
			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetTimeSeries_MeasurementLocationNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", null, TimeSeriesRange.OneMonth);

			await influxProvider.DidNotReceive().GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>());
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
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries1);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetTimeSeries("temperature", "Room1", TimeSeriesRange.OneDay);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObj = Assert.IsType<TimeSeriesResult>(okResult.Value);
			Assert.Equal(dataPoints, resultObj.DataPoints);
		}

		#endregion

		#region GetPreconfiguredTimeSeries

		[Fact]
		public async Task GetPreconfiguredTimeSeries_InternalError()
		{
			TimeSeriesResponse response1 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = new()
				}
			};
			TimeSeriesResponse response2 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.InternalError,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = null
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response1), Task.FromResult(response2));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries2);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetPreconfiguredTimeSeries(TimeSeriesRange.OneDay);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetPreconfiguredTimeSeries_BadRequest()
		{
			TimeSeriesResponse response1 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = new()
				}
			};
			TimeSeriesResponse response2 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.BadRequest,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = null
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response1), Task.FromResult(response2));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries2);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetPreconfiguredTimeSeries(TimeSeriesRange.OneDay);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetPreconfiguredTimeSeries_MeasurementNameNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries3);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetPreconfiguredTimeSeries(TimeSeriesRange.OneMonth);

			await influxProvider.DidNotReceive().GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>());
			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetPreconfiguredTimeSeries_MeasurementLocationNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries4);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetPreconfiguredTimeSeries(TimeSeriesRange.OneMonth);

			await influxProvider.DidNotReceive().GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>());
			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task GetPreconfiguredTimeSeries_Success()
		{
			var dataPoints1 = new List<DataPoint>()
			{
				new DataPoint<float>(new DateTime(2020, 10, 30), 3.14f),
				new DataPoint<float>(new DateTime(2020, 10, 31), 3.14f)
			};
			var dataPoints2 = new List<DataPoint>()
			{
				new DataPoint<float>(new DateTime(2020, 1, 30), 3.14f),
				new DataPoint<float>(new DateTime(2020, 1, 31), 3.14f)
			};
			TimeSeriesResponse response1 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = dataPoints1
				}
			};
			TimeSeriesResponse response2 = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				TimeSeriesResult = new()
				{
					DisplayName = "xyz",
					DataPoints = dataPoints2
				}
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>()).Returns(Task.FromResult(response1), Task.FromResult(response2));
			var optionsMonitor = Substitute.For<IOptionsMonitor<PreconfiguredTimeSeries>>();
			optionsMonitor.CurrentValue.Returns(preconfiguredTimeSeries2);
			TimeSeriesController controller = new TimeSeriesController(influxProvider, optionsMonitor);

			var result = await controller.GetPreconfiguredTimeSeries(TimeSeriesRange.OneDay);

			await influxProvider.Received(2).GetTimeSeriesAsync(Arg.Any<TimeSeriesRequest>(), Arg.Any<string>());
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObj = Assert.IsType<List<TimeSeriesResult>>(okResult.Value);
			Assert.Equal(2, resultObj.Count);
			Assert.Equal(dataPoints1, resultObj[0].DataPoints);
			Assert.Equal(dataPoints2, resultObj[1].DataPoints);
		}

		#endregion
	}
}
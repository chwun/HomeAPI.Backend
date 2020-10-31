using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Providers.TimeSeries;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class TimeSeriesControllerTests
	{
		#region GetTimeSeries

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeries_InternalError()
		{
			TimeSeriesRequest request = new TimeSeriesRequest()
			{
				MeasurementName = "temperature",
				ValueType = TimeSeriesValueType.Float,
				Range = TimeSeriesRange.OneDay,
				Tags = new Dictionary<string, string>()
				{
					["location"] = "Room1"
				}
			};

			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.InternalError,
				DataPoints = null
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(request).Returns(Task.FromResult(response));
			TimeSeriesController controller = new TimeSeriesController(influxProvider);

			var result = await controller.GetTimeSeries(request);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeries_BadRequest()
		{
			TimeSeriesRequest request = new TimeSeriesRequest()
			{
				MeasurementName = "temperature",
				ValueType = TimeSeriesValueType.Float,
				Range = TimeSeriesRange.OneDay,
				Tags = new Dictionary<string, string>()
				{
					["location"] = "Room1"
				}
			};

			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.BadRequest,
				DataPoints = null
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(request).Returns(Task.FromResult(response));
			TimeSeriesController controller = new TimeSeriesController(influxProvider);

			var result = await controller.GetTimeSeries(request);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeries_RequestNull()
		{
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			TimeSeriesController controller = new TimeSeriesController(influxProvider);

			var result = await controller.GetTimeSeries(null);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeries_SuccessAsync()
		{
			TimeSeriesRequest request = new TimeSeriesRequest()
			{
				MeasurementName = "temperature",
				ValueType = TimeSeriesValueType.Float,
				Range = TimeSeriesRange.OneDay,
				Tags = new Dictionary<string, string>()
				{
					["location"] = "Room1"
				}
			};
			var dataPoints = new List<DataPoint>()
			{
				new DataPoint<float>(new DateTime(2020, 10, 30), 3.14f),
				new DataPoint<float>(new DateTime(2020, 10, 31), 3.14f)
			};
			TimeSeriesResponse response = new TimeSeriesResponse()
			{
				Status = TimeSeriesResponseStatus.Success,
				DataPoints = dataPoints
			};
			IInfluxDBProvider influxProvider = Substitute.For<IInfluxDBProvider>();
			influxProvider.GetTimeSeriesAsync(request).Returns(Task.FromResult(response));
			TimeSeriesController controller = new TimeSeriesController(influxProvider);

			var result = await controller.GetTimeSeries(request);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultList = Assert.IsType<List<DataPoint>>(okResult.Value);
			Assert.Equal(dataPoints, resultList);
		}

		#endregion
	}
}
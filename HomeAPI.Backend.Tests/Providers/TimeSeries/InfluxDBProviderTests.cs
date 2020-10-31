using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Models.TimeSeries.InfluxDB;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.TimeSeries;
using HomeAPI.Backend.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Providers.TimeSeries
{
	public class InfluxDBProviderTests
	{
		#region GetTimeSeriesAsync

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeriesAsync_Exception()
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
			string httpResponseContent = "testHttpQueryResult";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(httpResponseContent, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<InfluxDBOptions>>();
			var influxOptions = new InfluxDBOptions()
			{
				Ip = "192.168.0.10",
				Port = 8086,
				Database = "TestDB"
			};
			optionsMonitor.CurrentValue.Returns(influxOptions);
			IFluxHelper fluxHelper = Substitute.For<IFluxHelper>();
			IInfluxDBQueryResultParser queryResultParser = Substitute.For<IInfluxDBQueryResultParser>();
			queryResultParser.ParseQueryResult(httpResponseContent, TimeSeriesValueType.Float).Returns(x => throw new Exception());
			InfluxDBProvider provider = new InfluxDBProvider(clientFactory, optionsMonitor, fluxHelper, queryResultParser);

			var result = await provider.GetTimeSeriesAsync(request);

			Assert.NotNull(result);
			Assert.Equal(TimeSeriesResponseStatus.InternalError, result.Status);
			Assert.Null(result.DataPoints);
		}

		[Fact]
		public async System.Threading.Tasks.Task GetTimeSeriesAsync_Success()
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
			string httpResponseContent = "testHttpQueryResult";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(httpResponseContent, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<InfluxDBOptions>>();
			var influxOptions = new InfluxDBOptions()
			{
				Ip = "192.168.0.10",
				Port = 8086,
				Database = "TestDB"
			};
			optionsMonitor.CurrentValue.Returns(influxOptions);
			var dataPoints = new List<DataPoint>()
			{
				new DataPoint<float>(new DateTime(2020, 10, 30), 3.14f),
				new DataPoint<float>(new DateTime(2020, 10, 31), 3.14f)
			};
			IFluxHelper fluxHelper = Substitute.For<IFluxHelper>();
			IInfluxDBQueryResultParser queryResultParser = Substitute.For<IInfluxDBQueryResultParser>();
			queryResultParser.ParseQueryResult(httpResponseContent, TimeSeriesValueType.Float).Returns(dataPoints);
			InfluxDBProvider provider = new InfluxDBProvider(clientFactory, optionsMonitor, fluxHelper, queryResultParser);

			var result = await provider.GetTimeSeriesAsync(request);

			Assert.NotNull(result);
			Assert.Equal(TimeSeriesResponseStatus.Success, result.Status);
			Assert.Equal(dataPoints, result.DataPoints);
		}

		#endregion
	}
}
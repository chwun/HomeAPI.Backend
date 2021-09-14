using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Models.TimeSeries.InfluxDB;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;

namespace HomeAPI.Backend.Providers.TimeSeries
{
	public class InfluxDBProvider : IInfluxDBProvider
	{
		private readonly IHttpClientFactory clientFactory;
		private readonly InfluxDBOptions options;
		private readonly IFluxHelper fluxHelper;
		private readonly IInfluxDBQueryResultParser queryResultParser;

		private readonly string apiUrl;

		public InfluxDBProvider(IHttpClientFactory clientFactory, IOptionsMonitor<InfluxDBOptions> optionsMonitor,
			IFluxHelper fluxHelper, IInfluxDBQueryResultParser queryResultParser)
		{
			this.clientFactory = clientFactory;
			options = optionsMonitor.CurrentValue;
			this.fluxHelper = fluxHelper;
			this.queryResultParser = queryResultParser;

			apiUrl = $"http://{options.Ip}";
			if (options.Port > 0)
			{
				apiUrl += $":{options.Port}";
			}

			apiUrl += "/api/v2";
		}

		public async Task<TimeSeriesResponse> GetTimeSeriesAsync(TimeSeriesRequest request, string displayName)
		{
			TimeSeriesResponse response = new TimeSeriesResponse();

			try
			{
				string url = $"{apiUrl}/query";

				var httpClient = clientFactory.CreateClient();

				var httpRequest = new HttpRequestMessage()
				{
					RequestUri = new Uri(url),
					Method = HttpMethod.Post
				};
				httpRequest.Headers.Add("Accept", "application/csv");

				string flux = fluxHelper.CreateQuery(options.Database, request);

				httpRequest.Content = new StringContent(
					flux,
					Encoding.UTF8,
					"application/vnd.flux"
				);

				var httpResponseMessage = await httpClient.SendAsync(httpRequest);
				string queryResult = await httpResponseMessage.Content.ReadAsStringAsync();

				response.Status = TimeSeriesResponseStatus.Success;
				response.TimeSeriesResult = new TimeSeriesResult()
				{
					DisplayName = displayName,
					DataPoints = queryResultParser.ParseQueryResult(queryResult, request.ValueType)
				};
			}
			catch
			{
				response.TimeSeriesResult = null;
				response.Status = TimeSeriesResponseStatus.InternalError;
			}

			return response;
		}
	}
}
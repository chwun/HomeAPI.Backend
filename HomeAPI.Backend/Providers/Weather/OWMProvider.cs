using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HomeAPI.Backend.Common;
using HomeAPI.Backend.Models.Weather;
using HomeAPI.Backend.Models.Weather.OpenWeatherMap;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HomeAPI.Backend.Providers.Weather
{
	public class OWMProvider : IOWMProvider
	{
		private readonly IHttpClientFactory clientFactory;
		private readonly OWMOptions options;
		private readonly string apiUrl;
		private readonly IDateTimeProvider dateTimeProvider;

		public OWMProvider(IHttpClientFactory clientFactory, IOptionsMonitor<OWMOptions> optionsMonitor, IDateTimeProvider dateTimeProvider)
		{
			this.clientFactory = clientFactory;
			options = optionsMonitor.CurrentValue;
			this.dateTimeProvider = dateTimeProvider;

			apiUrl = $"http://api.openweathermap.org/data/2.5/onecall?appid={options.ApiKey}";
		}

		public async Task<CurrentWeatherResponse> GetWeatherAsync()
		{
			StringBuilder url = new StringBuilder(apiUrl);
			url.AppendFormat("&lat={0}", options.Latitude);
			url.AppendFormat("&lon={0}", options.Longitude);
			url.AppendFormat("&exlude=hourly"); // only daily forecast is needed
			url.Append("&units=metric");
			url.AppendFormat("&lang={0}", options.LanguageCode);

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonResponse = await httpClient.GetStringAsync(url.ToString());

				var weatherData = JsonConvert.DeserializeObject<OWMCurrentWeatherResponse>(jsonResponse);

				return weatherData?.ToCurrentWeatherResponse();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<List<DailyWeatherData>> GetDailyForecastAsync()
		{
			var weather = await GetWeatherAsync();
			var daily = weather?.Daily?.OrderBy(x => x.Timestamp).Where(x => x.Timestamp.Date > dateTimeProvider.UtcNow.Date).ToList();

			return daily;
		}
	}
}
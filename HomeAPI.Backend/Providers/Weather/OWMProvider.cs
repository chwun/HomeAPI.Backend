using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

		public OWMProvider(IHttpClientFactory clientFactory, IOptionsMonitor<OWMOptions> optionsMonitor)
		{
			this.clientFactory = clientFactory;
			options = optionsMonitor.CurrentValue;

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
	}
}
using System.Collections.Generic;

namespace HomeAPI.Backend.Models.Weather
{
	public class CurrentWeatherResponse
	{
		public float Latitude { get; set; }

		public float Longitude { get; set; }

		public CurrentWeatherData Current { get; set; }

		public List<DailyWeatherData> Daily { get; set; }
	}
}
using System;

namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMDailyWeatherData
	{
		public long Dt { get; set; }

		public long Sunrise { get; set; }

		public long Sunset { get; set; }

		public OWMDayTemperatureSet Temp { get; set; }

		public OWMDayTemperatureSet Feels_Like { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		public float Uvi { get; set; }

		public int Clouds { get; set; }

		public float Wind_Speed { get; set; }

		public int Wind_Deg { get; set; }

		public OWMWeatherCondition[] Weather { get; set; }

		public DailyWeatherData ToDailyWeatherData()
		{
			return new DailyWeatherData()
			{
				Timestamp = DateTimeOffset.FromUnixTimeSeconds(Dt).UtcDateTime,
				Sunrise = DateTimeOffset.FromUnixTimeSeconds(Sunrise).UtcDateTime,
				Sunset = DateTimeOffset.FromUnixTimeSeconds(Sunset).UtcDateTime,
				Temperature = Temp.ToDayTemperatureSet(),
				TemperatureFeelsLike = Feels_Like.ToDayTemperatureSet(),
				Pressure = Pressure,
				Humidity = Humidity,
				UVIndex = Uvi,
				Clouds = Clouds,
				WindSpeed = Wind_Speed,
				WindDirection = Wind_Deg,
				Weather = Weather[0].ToWeatherCondition()
			};
		}
	}
}
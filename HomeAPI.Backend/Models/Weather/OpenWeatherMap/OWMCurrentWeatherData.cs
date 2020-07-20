using System;

namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMCurrentWeatherData
	{
		public long Dt { get; set; }

		public long Sunrise { get; set; }

		public long Sunset { get; set; }

		public float Temp { get; set; }

		public float Feels_Like { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		public float Uvi { get; set; }

		public int Clouds { get; set; }

		public int Visibility { get; set; }

		public float Wind_Speed { get; set; }

		public int Wind_Deg { get; set; }

		public OWMWeatherCondition[] Weather { get; set; }

		public CurrentWeatherData ToCurrentWeatherData()
		{
			return new CurrentWeatherData()
			{
				Timestamp = DateTimeOffset.FromUnixTimeSeconds(Dt).UtcDateTime,
				Sunrise = DateTimeOffset.FromUnixTimeSeconds(Sunrise).UtcDateTime,
				Sunset = DateTimeOffset.FromUnixTimeSeconds(Sunset).UtcDateTime,
				Temperature = Temp,
				TemperatureFeelsLike = Feels_Like,
				Pressure = Pressure,
				Humidity = Humidity,
				UVIndex = Uvi,
				Clouds = Clouds,
				Visibility = Visibility,
				WindSpeed = Wind_Speed,
				WindDirection = Wind_Deg,
				Weather = Weather[0].ToWeatherCondition()
			};
		}
	}
}
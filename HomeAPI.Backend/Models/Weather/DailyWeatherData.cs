using System;

namespace HomeAPI.Backend.Models.Weather
{
	public class DailyWeatherData
	{
		public DateTime Timestamp { get; set; }

		public DateTime Sunrise { get; set; }

		public DateTime Sunset { get; set; }

		public DayTemperatureSet Temperature { get; set; }

		public DayTemperatureSet TemperatureFeelsLike { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		public float UVIndex { get; set; }

		public int Clouds { get; set; }

		public float WindSpeed { get; set; }

		public int WindDirection { get; set; }

		public WeatherCondition Weather { get; set; }
	}
}
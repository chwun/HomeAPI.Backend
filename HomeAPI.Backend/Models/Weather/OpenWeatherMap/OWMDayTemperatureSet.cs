namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMDayTemperatureSet
	{
		public float Morn { get; set; }

		public float Day { get; set; }

		public float Eve { get; set; }

		public float Night { get; set; }

		public float Min { get; set; }

		public float Max { get; set; }

		public DayTemperatureSet ToDayTemperatureSet()
		{
			return new DayTemperatureSet()
			{
				TemperatureMorning = Morn,
				TemperatureDay = Day,
				TemperatureEvening = Eve,
				TemperatureNight = Night,
				TemperatureMin = Min,
				TemperatureMax = Max
			};
		}
	}
}
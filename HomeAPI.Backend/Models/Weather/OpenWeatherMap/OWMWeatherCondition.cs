namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMWeatherCondition
	{
		public int Id { get; set; }

		public string Main { get; set; }

		public string Description { get; set; }

		public string Icon { get; set; }

		public WeatherCondition ToWeatherCondition()
		{
			return new WeatherCondition()
			{
				Description = Description,
				IconId = Icon
			};
		}
	}
}
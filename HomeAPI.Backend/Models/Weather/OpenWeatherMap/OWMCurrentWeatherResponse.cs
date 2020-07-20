using System.Linq;

namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMCurrentWeatherResponse
	{
		public float Lat { get; set; }

		public float Lon { get; set; }

		public OWMCurrentWeatherData Current { get; set; }

		public OWMDailyWeatherData[] Daily { get; set; }

		public CurrentWeatherResponse ToCurrentWeatherResponse()
		{
			return new CurrentWeatherResponse()
			{
				Latitude = Lat,
				Longitude = Lon,
				Current = Current.ToCurrentWeatherData(),
				Daily = Daily.Select(x => x.ToDailyWeatherData()).ToList()
			};
		}
	}
}
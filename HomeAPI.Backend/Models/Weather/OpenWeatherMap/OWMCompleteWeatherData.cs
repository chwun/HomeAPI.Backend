using System.Collections.Generic;
using System.Linq;

namespace HomeAPI.Backend.Models.Weather.OpenWeatherMap
{
	public class OWMCompleteWeatherData
	{
		public float Lat { get; set; }

		public float Lon { get; set; }

		public OWMCurrentWeatherData Current { get; set; }

		public OWMDailyWeatherData[] Daily { get; set; }

		public CompleteWeatherData ToCompleteWeatherData()
		{
			return new CompleteWeatherData()
			{
				Latitude = Lat,
				Longitude = Lon,
				Current = Current.ToCurrentWeatherData(),
				Daily = Daily?.Select(x => x.ToDailyWeatherData()).ToList() ?? new List<DailyWeatherData>()
			};
		}
	}
}
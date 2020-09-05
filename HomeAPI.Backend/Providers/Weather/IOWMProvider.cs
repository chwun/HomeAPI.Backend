using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Weather;

namespace HomeAPI.Backend.Providers.Weather
{
	public interface IOWMProvider
	{
		Task<CompleteWeatherData> GetCompleteWeatherAsync();

		Task<CurrentWeatherData> GetCurrentWeatherAsync();

		Task<List<DailyWeatherData>> GetDailyForecastAsync();
	}
}
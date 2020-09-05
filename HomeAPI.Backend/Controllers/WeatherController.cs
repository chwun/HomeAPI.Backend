using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Weather;
using HomeAPI.Backend.Providers.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherController : ControllerBase
	{
		private readonly IOWMProvider owmProvider;

		public WeatherController(IOWMProvider owmProvider)
		{
			this.owmProvider = owmProvider;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CompleteWeatherData>> GetWeather()
		{
			var weather = await owmProvider.GetCompleteWeatherAsync();

			if (weather == null)
			{
				return NotFound();
			}

			return Ok(weather);
		}

		[HttpGet("current")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CurrentWeatherData>> GetCurrentWeather()
		{
			var currentWeather = await owmProvider.GetCurrentWeatherAsync();

			if (currentWeather == null)
			{
				return NotFound();
			}

			return Ok(currentWeather);
		}

		[HttpGet("forecast/daily")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<DailyWeatherData>>> GetDailyForecast()
		{
			var dailyForecast = await owmProvider.GetDailyForecastAsync();

			if ((dailyForecast == null) || (dailyForecast.Count == 0))
			{
				return NotFound();
			}

			return Ok(dailyForecast);
		}
	}
}
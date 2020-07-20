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
		public async Task<ActionResult<CurrentWeatherResponse>> GetWeather()
		{
			var weather = await owmProvider.GetWeatherAsync();

			if (weather == null)
			{
				return NotFound();
			}

			return Ok(weather);
		}
	}
}
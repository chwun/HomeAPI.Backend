using System.Threading.Tasks;
using HomeAPI.Backend.Providers;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LightingController : ControllerBase
	{
		private readonly IHueProvider hueProvider;

		public LightingController(IHueProvider hueProvider)
		{
			this.hueProvider = hueProvider;
		}

		[HttpGet("test")]
		public ActionResult<string> Test([FromQuery]string name)
		{
			return "Hello " + name;
		}

		[HttpGet("availablelights")]
		public async Task<ActionResult<string>> GetAvailableLightsAsync()
		{
			string result = await hueProvider.GetAvailableLightsAsync();

			return result;
		}
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;
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

		[HttpGet("lights")]
		public async Task<IEnumerable<Light>> GetAllLights()
		{
			return await hueProvider.GetAllLightsAsync();
		}
	}
}
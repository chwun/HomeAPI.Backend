using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Providers;
using Microsoft.AspNetCore.Http;
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

		[HttpGet("lights")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<Light>>> GetAllLights()
		{
			var lights = await hueProvider.GetAllLightsAsync();

			if (lights == null || lights.Count == 0)
			{
				return NoContent();
			}

			return lights;
		}
	}
}
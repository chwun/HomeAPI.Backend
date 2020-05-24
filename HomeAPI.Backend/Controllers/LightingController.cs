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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<Light>>> GetAllLights()
		{
			var lights = await hueProvider.GetAllLightsAsync();

			if (lights == null)
			{
				return NotFound();
			}

			if (lights.Count == 0)
			{
				return NoContent();
			}

			return Ok(lights);
		}

		[HttpGet("lights/{id:int}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Light>> GetLight(int id)
		{
			var light = await hueProvider.GetLightByIdAsync(id);

			if (light == null)
			{
				return NotFound();
			}

			return Ok(light);
		}

		[HttpPut("lights/{id:int}/state")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> SetLightState(int id, [FromBody] LightStateUpdate stateUpdate)
		{
			bool success = await hueProvider.SetLightStateAsync(id, stateUpdate);

			if (success)
			{
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}
	}
}
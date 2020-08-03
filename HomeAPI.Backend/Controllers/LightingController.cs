using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Providers.Lighting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LightingController : ControllerBase
	{
		private readonly IHueLightProvider hueProvider;
		private readonly IAsyncRepository<LightScene> lightSceneRepository;

		public LightingController(IHueLightProvider hueProvider, IAsyncRepository<LightScene> lightSceneRepository)
		{
			this.lightSceneRepository = lightSceneRepository;
			this.hueProvider = hueProvider;
		}

		[HttpGet("lights")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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

		[HttpGet("scenes")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<LightScene>>> GetLightScenes()
		{
			var scenes = await lightSceneRepository.GetAllAsync();

			if (scenes == null)
			{
				return NotFound();
			}

			return Ok(scenes);
		}

		[HttpGet("scenes/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LightScene>> GetLightScene(int id)
		{
			var scene = await lightSceneRepository.GetAsync(id);

			if (scene == null)
			{
				return NotFound();
			}

			return Ok(scene);
		}

		[HttpPost("scenes")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LightScene>> AddLightScene([FromBody] LightScene scene)
		{
			if (scene == null)
			{
				return BadRequest();
			}

			// id is set via auto-increment:
			scene.Id = 0;

			await lightSceneRepository.AddAsync(scene);

			return CreatedAtAction(nameof(GetLightScene), new { id = scene.Id }, scene);
		}

		[HttpPut("scenes/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LightScene>> UpdateLightScene(int id, [FromBody] LightScene modifiedScene)
		{
			if ((modifiedScene == null) || (id != modifiedScene.Id))
			{
				return BadRequest();
			}

			await lightSceneRepository.UpdateAsync(modifiedScene);

			return Ok(modifiedScene);
		}

		[HttpDelete("scenes/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LightScene>> DeleteLightScene(int id)
		{
			var scene = await lightSceneRepository.GetAsync(id);
			if (scene == null)
			{
				return NotFound();
			}

			await lightSceneRepository.RemoveAsync(scene);

			return Ok(scene);
		}

		[HttpGet("scenes/{id:int}/active")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ApplyLightScene(int id)
		{
			var scene = await lightSceneRepository.GetAsync(id);

			if (scene == null)
			{
				return NotFound();
			}

			bool success = await hueProvider.ApplyLightSceneAsync(scene);

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
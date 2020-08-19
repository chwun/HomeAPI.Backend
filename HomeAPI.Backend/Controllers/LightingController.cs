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
		private readonly IHueLightProvider hueLightProvider;
		private readonly IAsyncRepository<LightScene> lightSceneRepository;

		public LightingController(IHueLightProvider hueLightProvider, IAsyncRepository<LightScene> lightSceneRepository)
		{
			this.lightSceneRepository = lightSceneRepository;
			this.hueLightProvider = hueLightProvider;
		}

		[HttpGet("lights")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<Light>>> GetAllLights()
		{
			var lights = await hueLightProvider.GetAllLightsAsync();

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

		[HttpGet("lights/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Light>> GetLight(int id)
		{
			var light = await hueLightProvider.GetLightByIdAsync(id);

			if (light == null)
			{
				return NotFound();
			}

			return Ok(light);
		}

		[HttpPut("lights/{id}/state")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> SetLightState(int id, [FromBody] LightStateUpdate stateUpdate)
		{
			bool success = await hueLightProvider.SetLightStateAsync(id, stateUpdate);

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

		[HttpGet("scenes/{id}")]
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

		[HttpPut("scenes/{id}")]
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

		[HttpDelete("scenes/{id}")]
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

		[HttpGet("scenes/{id}/active")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ApplyLightScene(int id)
		{
			var scene = await lightSceneRepository.GetAsync(id);

			if (scene == null)
			{
				return NotFound();
			}

			bool success = await hueLightProvider.ApplyLightSceneAsync(scene);

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
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Sensors;
using HomeAPI.Backend.Providers.Sensors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAPI.Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MeasurementController : ControllerBase
	{
		private readonly IHueSensorProvider hueSensorProvider;

		public MeasurementController(IHueSensorProvider hueSensorProvider)
		{
			this.hueSensorProvider = hueSensorProvider;
		}

		[HttpGet("sensors")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<Sensor>>> GetAllSensors()
		{
			var sensors = await hueSensorProvider.GetAllSensorsAsync();

			if (sensors == null)
			{
				return NotFound();
			}

			if (sensors.Count == 0)
			{
				return NoContent();
			}

			return Ok(sensors);
		}

		[HttpGet("sensors/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Sensor>> GetSensor(int id)
		{
			var sensor = await hueSensorProvider.GetSensorByIdAsync(id);

			if (sensor == null)
			{
				return NotFound();
			}

			return Ok(sensor);
		}

		[HttpGet("sensors/temperature")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<Sensor>>> GetAllTemperatureSensors()
		{
			var sensors = await hueSensorProvider.GetAllTemperatureSensorsAsync();

			if (sensors == null)
			{
				return NotFound();
			}

			if (sensors.Count == 0)
			{
				return NoContent();
			}

			return Ok(sensors);
		}

		[HttpGet("sensors/temperature/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Sensor>> GetTemperatureSensor(int id)
		{
			var sensor = await hueSensorProvider.GetTemperatureSensorByIdAsync(id);

			if (sensor == null)
			{
				return NotFound();
			}

			return Ok(sensor);
		}
	}
}
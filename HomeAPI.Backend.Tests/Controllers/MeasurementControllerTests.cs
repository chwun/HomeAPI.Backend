using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Models.Sensors;
using HomeAPI.Backend.Providers.Sensors;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class MeasurementControllerTests
	{
		#region GetAllSensors

		[Fact]
		public async Task GetAllSensors_ListNull()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<Sensor> list = null;
			hueSensorProvider.GetAllSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllSensors();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetAllSensors_ListEmpty()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<Sensor> list = new List<Sensor>();
			hueSensorProvider.GetAllSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllSensors();

			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllSensors_ListValid()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<Sensor> list = new List<Sensor>
			{
				new DummySensor
				{
					Id = 1,
					Name = "Sensor1",
					Type = SensorType.Dummy,
					BatteryPercentage = 51
				},
				new TemperatureSensor
				{
					Id = 6,
					Name = "Sensor6",
					Type = SensorType.Temperature,
					BatteryPercentage = 51,
					State = new TemperatureSensorState
					{
						Temperature = 24.57f
					}
				}
			};
			hueSensorProvider.GetAllSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllSensors();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var sensors = Assert.IsType<List<Sensor>>(okResult.Value);
			Assert.Equal(list, sensors);
		}

		#endregion

		#region GetSensor

		[Fact]
		public async Task GetSensor_NotFound()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			Sensor sensor = null;
			hueSensorProvider.GetSensorByIdAsync(1).Returns(Task.FromResult(sensor));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetSensor(1);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetSensor_Found()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			Sensor sensor = new DummySensor
			{
				Id = 1,
				Name = "Sensor1",
				Type = SensorType.Dummy,
				BatteryPercentage = 51
			};
			hueSensorProvider.GetSensorByIdAsync(1).Returns(Task.FromResult(sensor));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetSensor(1);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var sensorResult = Assert.IsType<DummySensor>(okResult.Value);
			Assert.Equal(sensor, sensorResult);
		}

		#endregion

		#region GetAllTemperatureSensors

		[Fact]
		public async Task GetAllTemperatureSensors_ListNull()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<TemperatureSensor> list = null;
			hueSensorProvider.GetAllTemperatureSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllTemperatureSensors();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetAllTemperatureSensors_ListEmpty()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<TemperatureSensor> list = new List<TemperatureSensor>();
			hueSensorProvider.GetAllTemperatureSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllTemperatureSensors();

			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllTemperatureSensors_ListValid()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			List<TemperatureSensor> list = new List<TemperatureSensor>
			{
				new TemperatureSensor
				{
					Id = 1,
					Name = "Sensor1",
					Type = SensorType.Temperature,
					BatteryPercentage = 59,
					State = new TemperatureSensorState
					{
						Temperature = 21.07f
					}
				},
				new TemperatureSensor
				{
					Id = 6,
					Name = "Sensor6",
					Type = SensorType.Temperature,
					BatteryPercentage = 51,
					State = new TemperatureSensorState
					{
						Temperature = 24.57f
					}
				}
			};
			hueSensorProvider.GetAllTemperatureSensorsAsync().Returns(Task.FromResult(list));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetAllTemperatureSensors();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var sensors = Assert.IsType<List<TemperatureSensor>>(okResult.Value);
			Assert.Equal(list, sensors);
		}

		#endregion

		#region GetTemperatureSensor

		[Fact]
		public async Task GetTemperatureSensor_NotFound()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			TemperatureSensor sensor = null;
			hueSensorProvider.GetTemperatureSensorByIdAsync(1).Returns(Task.FromResult(sensor));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetTemperatureSensor(1);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetTemperatureSensor_Found()
		{
			var hueSensorProvider = Substitute.For<IHueSensorProvider>();
			TemperatureSensor sensor = new TemperatureSensor
			{
				Id = 6,
				Name = "Sensor6",
				Type = SensorType.Temperature,
				BatteryPercentage = 51,
				State = new TemperatureSensorState
				{
					Temperature = 24.57f
				}
			};
			hueSensorProvider.GetTemperatureSensorByIdAsync(6).Returns(Task.FromResult(sensor));
			var controller = new MeasurementController(hueSensorProvider);

			var result = await controller.GetTemperatureSensor(6);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var sensorResult = Assert.IsType<TemperatureSensor>(okResult.Value);
			Assert.Equal(sensor, sensorResult);
		}

		#endregion
	}
}
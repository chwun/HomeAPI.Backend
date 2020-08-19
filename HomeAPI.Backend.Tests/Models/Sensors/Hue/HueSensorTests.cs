using System;
using HomeAPI.Backend.Models.Sensors;
using HomeAPI.Backend.Models.Sensors.Hue;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.Sensors.Hue
{

	public class HueSensorTests
	{
		[Fact]
		public void ToSensor_Temperature()
		{
			HueSensor hueSensor = new HueSensor
			{
				Name = "Sensor1",
				Type = "ZLLTemperature",
				State = new HueSensorState
				{
					Temperature = 2410,
					LastUpdated = new DateTime(2020, 08, 18, 16, 09, 25)
				},
				Config = new HueSensorConfig
				{
					Battery = 60,
					Reachable = true
				}
			};

			var result = hueSensor.ToSensor(6) as TemperatureSensor;

			Assert.NotNull(result);
			Assert.Equal(6, result.Id);
			Assert.Equal("Sensor1", result.Name);
			Assert.Equal(SensorType.Temperature, result.Type);
			Assert.Equal(60, result.BatteryPercentage);
			Assert.Equal(24.10f, result.State.Temperature);
		}

		[Fact]
		public void ToSensor_Dummy()
		{
			HueSensor hueSensor = new HueSensor
			{
				Name = "Sensor234",
				Type = "OtherType",
				State = new HueSensorState
				{
					LastUpdated = new DateTime(2020, 08, 18, 16, 09, 25)
				},
				Config = new HueSensorConfig
				{
					Battery = 60,
					Reachable = true
				}
			};

			var result = hueSensor.ToSensor(6) as DummySensor;

			Assert.NotNull(result);
			Assert.Equal(6, result.Id);
			Assert.Equal("Sensor234", result.Name);
			Assert.Equal(SensorType.Dummy, result.Type);
			Assert.Equal(60, result.BatteryPercentage);
		}
	}
}
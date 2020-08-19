using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Sensors.Hue
{
	public class HueSensor
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("state")]
		public HueSensorState State { get; set; }

		[JsonProperty("config")]
		public HueSensorConfig Config { get; set; }

		public Sensor ToSensor(int id)
		{
			Sensor sensor = null;

			switch (Type.ToUpper())
			{
				case "ZLLTEMPERATURE":
					sensor = new TemperatureSensor
					{
						Id = id,
						Name = Name,
						Type = SensorType.Temperature,
						BatteryPercentage = Config.Battery,
						State = new TemperatureSensorState
						{
							Temperature = State.Temperature / 100f
						}
					};

					break;

				default:
					sensor = new DummySensor()
					{
						Id = id,
						Name = Name,
						Type = SensorType.Dummy,
						BatteryPercentage = Config.Battery
					};

					break;
			}

			return sensor;
		}
	}
}
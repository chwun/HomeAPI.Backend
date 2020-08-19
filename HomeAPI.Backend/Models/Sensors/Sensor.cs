using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeAPI.Backend.Models.Sensors
{
	public abstract class Sensor
	{
		public int Id { get; set; }

		public string Name { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public SensorType Type { get; set; }

		public int BatteryPercentage { get; set; }
	}
}
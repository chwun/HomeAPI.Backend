using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Sensors.Hue
{
	public class HueSensorConfig
	{
		[JsonProperty("battery")]
		public int Battery { get; set; }

		[JsonProperty("reachable")]
		public bool Reachable { get; set; }
	}
}
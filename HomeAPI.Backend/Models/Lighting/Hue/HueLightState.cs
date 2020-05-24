using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightState
	{
		[JsonProperty("on")]
		public bool On { get; set; }

		[JsonProperty("bri")]
		public int Bri { get; set; }

		[JsonProperty("sat")]
		public int Sat { get; set; }

		[JsonProperty("hue")]
		public int Hue { get; set; }

		[JsonProperty("ct")]
		public int Ct { get; set; }

		[JsonProperty("reachable")]
		public bool Reachable { get; set; }
	}
}
using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightStateUpdateExtendedColor : HueLightStateUpdate
	{
		[JsonProperty("bri")]
		public int Brightness { get; set; }

		[JsonProperty("hue")]
		public int Hue { get; set; }

		[JsonProperty("sat")]
		public int Saturation { get; set; }

		[JsonProperty("ct")]
		public int ColorTemperature { get; set; }
	}
}
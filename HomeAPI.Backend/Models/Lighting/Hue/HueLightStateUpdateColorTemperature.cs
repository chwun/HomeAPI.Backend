using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightStateUpdateColorTemperature : HueLightStateUpdate
	{
		[JsonProperty("bri")]
		public int Brightness { get; set; }

		[JsonProperty("ct")]
		public int ColorTemperature { get; set; }
	}
}
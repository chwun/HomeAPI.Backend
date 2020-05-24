using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightStateUpdateColorTemperature : HueLightStateUpdate
	{
		[JsonProperty("ct")]
		public int ColorTemperature { get; set; }
	}
}
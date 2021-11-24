using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightStateUpdateDimmable : HueLightStateUpdate
	{
		[JsonProperty("bri")]
		public int Brightness { get; set; }
	}
}
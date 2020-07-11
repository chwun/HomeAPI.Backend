using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueOnOffLightStateUpdate
	{
		[JsonProperty("on")]
		public bool On { get; set; }
	}
}
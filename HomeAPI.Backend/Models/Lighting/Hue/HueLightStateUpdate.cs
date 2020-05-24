using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public abstract class HueLightStateUpdate
	{
		[JsonProperty("on")]
		public bool On { get; set; }

		[JsonProperty("bri")]
		public int Brightness { get; set; }
	}
}
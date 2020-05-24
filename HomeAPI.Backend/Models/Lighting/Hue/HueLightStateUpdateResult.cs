using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLightStateUpdateResult
	{
		[JsonProperty("success")]
		public object Success { get; set; }

		[JsonProperty("error")]
		public object Error { get; set; }
	}
}
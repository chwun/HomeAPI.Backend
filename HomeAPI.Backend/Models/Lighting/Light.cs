using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeAPI.Backend.Models.Lighting
{
	public class Light
	{
		public int Id { get; set; }

		public string Name { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public LightType Type { get; set; }

		public LightState State { get; set; }
	}
}
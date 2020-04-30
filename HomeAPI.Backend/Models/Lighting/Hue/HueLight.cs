using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	public class HueLight : ILightFactory
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("state")]
		public HueLightState State { get; set; }

		/// <summary>
		/// Creates a new common light object from this light object
		/// </summary>
		/// <param name="id">id of the new created light object</param>
		/// <returns>this light as common light object</returns>
		public Light ToLight(int id)
		{
			return new Light()
			{
				Id = id,
				Name = this.Name,
				Type = this.Type.ToLower() switch
				{
					"dimmable light" => LightType.HueDimmableLight,
					"extended color light" => LightType.HueExtendedColorLight,
					"color temperature light" => LightType.HueColorTemperatureLight,
					_ => LightType.Undefined
				},
				State = new LightState()
				{
					On = this.State.On,
					Brightness = this.State.Bri,
					Saturation = this.State.Sat,
					ColorTemperature = this.State.Ct,
					Reachable = this.State.Reachable
				}
			};
		}
	}
}
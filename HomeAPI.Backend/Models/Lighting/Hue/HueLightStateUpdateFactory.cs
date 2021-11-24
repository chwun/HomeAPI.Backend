using System;

namespace HomeAPI.Backend.Models.Lighting.Hue
{
	/// <summary>
	/// Factory class for creating instances of HueLightStateUpdate
	/// </summary>
	public class HueLightStateUpdateFactory : IHueLightStateUpdateFactory
	{
		/// <summary>
		/// Creates a new instance of HueLightStateUpdate from the given LightStateUpdate object
		/// </summary>
		/// <param name="lightType">type of the light</param>
		/// <param name="stateUpdate">light state update</param>
		/// <returns>new instance of HueLightStateUpdate</returns>
		public HueLightStateUpdate CreateFromLightState(LightType lightType, LightStateUpdate stateUpdate)
		{
			if (stateUpdate == null)
			{
				return null;
			}

			return lightType switch
			{
				LightType.HueDimmableLight => CreateDimmableLightStateUpdate(stateUpdate),
				LightType.HueExtendedColorLight => CreateExtendedColorLightStateUpdate(stateUpdate),
				LightType.HueColorTemperatureLight => CreateColorTemperatureLightStateUpdate(stateUpdate),
				LightType.OnOffPlug => CreateOnOffLightStateUpdateFromLightState(stateUpdate),
				_ => null
			};
		}

		public HueLightStateUpdateOnOff CreateOnOffLightStateUpdateFromLightState(LightStateUpdate stateUpdate)
		{
			if (stateUpdate == null)
			{
				return null;
			}

			return new HueLightStateUpdateOnOff() { On = stateUpdate.On };
		}

		private HueLightStateUpdateDimmable CreateDimmableLightStateUpdate(LightStateUpdate stateUpdate)
		{
			return new HueLightStateUpdateDimmable()
			{
				On = stateUpdate.On,
				Brightness = stateUpdate.Brightness
			};
		}

		private HueLightStateUpdateExtendedColor CreateExtendedColorLightStateUpdate(LightStateUpdate stateUpdate)
		{
			return new HueLightStateUpdateExtendedColor()
			{
				On = stateUpdate.On,
				Brightness = stateUpdate.Brightness,
				Saturation = stateUpdate.Saturation,
				Hue = stateUpdate.Hue,
				ColorTemperature = stateUpdate.ColorTemperature
			};
		}


		private HueLightStateUpdateColorTemperature CreateColorTemperatureLightStateUpdate(LightStateUpdate stateUpdate)
		{
			return new HueLightStateUpdateColorTemperature()
			{
				On = stateUpdate.On,
				Brightness = stateUpdate.Brightness,
				ColorTemperature = stateUpdate.ColorTemperature
			};
		}
	}
}
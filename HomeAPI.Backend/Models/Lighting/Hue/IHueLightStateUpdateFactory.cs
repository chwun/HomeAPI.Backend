namespace HomeAPI.Backend.Models.Lighting.Hue
{
	/// <summary>
	/// Factory interface for creating instances of HueLightStateUpdate
	/// </summary>
	public interface IHueLightStateUpdateFactory
	{
		/// <summary>
		/// Creates a new instance of HueLightStateUpdate from the given LightStateUpdate object
		/// </summary>
		/// <param name="lightType">type of the light</param>
		/// <param name="stateUpdate">light state update</param>
		/// <returns>new instance of HueLightStateUpdate</returns>
		HueLightStateUpdate CreateFromLightState(LightType lightType, LightStateUpdate stateUpdate);

		HueOnOffLightStateUpdate CreateOnOffLightStateUpdateFromLightState(LightStateUpdate stateUpdate);
	}
}
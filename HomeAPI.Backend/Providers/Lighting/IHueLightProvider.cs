using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;

namespace HomeAPI.Backend.Providers.Lighting
{
	public interface IHueLightProvider
	{
		Task<List<Light>> GetAllLightsAsync();

		Task<Light> GetLightByIdAsync(int lightId);

		Task<bool> SetLightStateAsync(int lightId, LightStateUpdate stateUpdate);

		Task<bool> ApplyLightSceneAsync(LightScene scene);
	}
}
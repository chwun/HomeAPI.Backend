using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;

namespace HomeAPI.Backend.Providers
{
	public interface IHueProvider
	{
		Task<List<Light>> GetAllLightsAsync();
	}
}
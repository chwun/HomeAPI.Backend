using System.Threading.Tasks;

namespace HomeAPI.Backend.Providers
{
	public interface IHueProvider
	{
		Task<string> GetAvailableLightsAsync();
	}
}
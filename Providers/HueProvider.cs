using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;

namespace HomeAPI.Backend.Providers
{
	public class HueProvider : IHueProvider
	{
		private readonly HueOptions options;
		private readonly HttpClient httpClient;
		private readonly string apiUrl;

		public HueProvider(IOptionsMonitor<HueOptions> optionsAccessor)
		{
			options = optionsAccessor.CurrentValue;
			httpClient = new HttpClient();

			apiUrl = $"http://{options.BridgeIP}";
			if (options.BridgePort > 0)
			{
				apiUrl += $":{options.BridgePort}";
			}

			apiUrl += $"/api/{options.UserKey}";
		}

		public Task<string> GetAvailableLightsAsync()
		{
			string url = $"{apiUrl}/lights";
			
			return httpClient.GetStringAsync(url);
		}
	}
}
using System.Net.Http;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;

namespace HomeAPI.Backend.Providers
{
	public abstract class HueProvider
	{
		protected readonly IHttpClientFactory clientFactory;
		protected readonly HueOptions options;
		protected readonly string apiUrl;

		protected HueProvider(IHttpClientFactory clientFactory, IOptionsMonitor<HueOptions> optionsMonitor)
		{
			this.clientFactory = clientFactory;
			options = optionsMonitor.CurrentValue;

			apiUrl = $"http://{options.BridgeIP}";
			if (options.BridgePort > 0)
			{
				apiUrl += $":{options.BridgePort}";
			}

			apiUrl += $"/api/{options.UserKey}";
		}
	}
}
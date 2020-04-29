using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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

		public async Task<List<Light>> GetAllLightsAsync()
		{
			string url = $"{apiUrl}/lights";

			var jsonText = await httpClient.GetStringAsync(url);
			var lightsDict = (Dictionary<int, HueLight>)JsonConvert.DeserializeObject(jsonText, typeof(Dictionary<int, HueLight>));

			List<Light> result = new List<Light>();

			foreach (KeyValuePair<int, HueLight> keyValue in lightsDict)
			{
				Light light = keyValue.Value.ToLight(keyValue.Key);
				result.Add(light);
			}

			return result.OrderBy(x => x.Id).ToList();
		}
	}
}
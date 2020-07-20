using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HomeAPI.Backend.Providers.Lighting
{
	public class HueProvider : IHueProvider
	{
		private readonly IHttpClientFactory clientFactory;
		private readonly HueOptions options;
		private readonly IHueLightStateUpdateFactory lightStateUpdateFactory;
		private readonly string apiUrl;

		public HueProvider(IHttpClientFactory clientFactory, IOptionsMonitor<HueOptions> optionsMonitor, IHueLightStateUpdateFactory lightStateUpdateFactory)
		{
			this.clientFactory = clientFactory;
			options = optionsMonitor.CurrentValue;
			this.lightStateUpdateFactory = lightStateUpdateFactory;

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

			List<Light> result = new List<Light>();

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);
				var lightsDict = JsonConvert.DeserializeObject<Dictionary<int, HueLight>>(jsonText);

				foreach (KeyValuePair<int, HueLight> keyValue in lightsDict)
				{
					Light light = keyValue.Value.ToLight(keyValue.Key);
					result.Add(light);
				}
			}
			catch (Exception)
			{
			}

			return result.OrderBy(x => x.Id).ToList();
		}

		public async Task<Light> GetLightByIdAsync(int lightId)
		{
			string url = $"{apiUrl}/lights/{lightId}";

			Light result = null;

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);

				var hueLight = JsonConvert.DeserializeObject<HueLight>(jsonText);

				result = hueLight.ToLight(lightId);
			}
			catch (Exception)
			{
			}

			return result;
		}

		public async Task<bool> SetLightStateAsync(int lightId, LightStateUpdate stateUpdate)
		{
			string url = $"{apiUrl}/lights/{lightId}/state";

			bool success = false;

			try
			{
				var light = await GetLightByIdAsync(lightId);

				var httpClient = clientFactory.CreateClient();

				// first send only on/off state to Hue bridge, because otherwise Hue will respond with error:
				var hueOnOffLightStateUpdate = lightStateUpdateFactory.CreateOnOffLightStateUpdateFromLightState(stateUpdate);
				var jsonOnOff = JsonConvert.SerializeObject(hueOnOffLightStateUpdate);
				await httpClient.PutAsync(url, new StringContent(jsonOnOff));

				// only proceed if light is switched on, because otherwise Hue will respond with error:
				if (hueOnOffLightStateUpdate.On)
				{
					var hueLightStateUpdate = lightStateUpdateFactory.CreateFromLightState(light.Type, stateUpdate);
					var json = JsonConvert.SerializeObject(hueLightStateUpdate);
					var response = await httpClient.PutAsync(url, new StringContent(json));

					var responseJson = await response.Content.ReadAsStringAsync();

					var updateResults = JsonConvert.DeserializeObject<HueLightStateUpdateResult[]>(responseJson);

					success = (updateResults.Length > 0) ? (updateResults[0].Success != null) : false;
				}
				else
				{
					success = true;
				}
			}
			catch (Exception)
			{
			}

			return success;
		}

		public async Task<bool> ApplyLightSceneAsync(LightScene scene)
		{
			bool success = true;

			try
			{
				var lightStateUpdates = JsonConvert.DeserializeObject<Dictionary<int, LightStateUpdate>>(scene.Data);

				foreach (var keyValue in lightStateUpdates)
				{
					success &= await SetLightStateAsync(keyValue.Key, keyValue.Value);
				}
			}
			catch (Exception)
			{
				success = false;
			}

			return success;
		}
	}
}
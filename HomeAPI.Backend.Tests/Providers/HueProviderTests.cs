using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers;
using HomeAPI.Backend.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Providers
{
    public class HueProviderTests
    {
		[Fact]
        public async Task GetAllLightsAsync_Successful()
		{
			string response = "{\"1\":{\"state\":{\"on\":false,\"bri\":126,\"alert\":\"none\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2020-03-03T13:27:12\"},\"type\":\"Dimmable light\",\"name\":\"Flur\",\"modelid\":\"LWB010\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue white lamp\",\"capabilities\":{\"certified\":true,\"control\":{\"mindimlevel\":5000,\"maxlumen\":806},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\",\"startup\":{\"mode\":\"safety\",\"configured\":true}},\"uniqueid\":\"00:17:88:01:02:b0:1b:3d-0b\",\"swversion\":\"1.50.2_r30933\",\"swconfigid\":\"1D8EE00F\",\"productid\":\"Philips-LWB010-1-A19DLv3\"},\"2\":{\"state\":{\"on\":false,\"bri\":88,\"alert\":\"none\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2020-03-03T13:27:17\"},\"type\":\"Dimmable light\",\"name\":\"Küche\",\"modelid\":\"LWB010\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue white lamp\",\"capabilities\":{\"certified\":true,\"control\":{\"mindimlevel\":5000,\"maxlumen\":806},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\",\"startup\":{\"mode\":\"safety\",\"configured\":true}},\"uniqueid\":\"00:17:88:01:02:b2:1a:03-0b\",\"swversion\":\"1.50.2_r30933\",\"swconfigid\":\"1D8EE00F\",\"productid\":\"Philips-LWB010-1-A19DLv3\"},\"3\":{\"state\":{\"on\":false,\"bri\":254,\"hue\":8010,\"sat\":172,\"effect\":\"none\",\"xy\":[0.4806,0.4134],\"ct\":406,\"alert\":\"none\",\"colormode\":\"hs\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2020-03-03T13:27:22\"},\"type\":\"Extended color light\",\"name\":\"Wohnzimmer (Lightstrip)\",\"modelid\":\"LST002\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue lightstrip plus\",\"capabilities\":{\"certified\":true,\"control\":{\"mindimlevel\":25,\"maxlumen\":1600,\"colorgamuttype\":\"C\",\"colorgamut\":[[0.6915,0.3083],[0.1700,0.7000],[0.1532,0.0475]],\"ct\":{\"min\":153,\"max\":500}},\"streaming\":{\"renderer\":true,\"proxy\":true}},\"config\":{\"archetype\":\"huelightstrip\",\"function\":\"mixed\",\"direction\":\"omnidirectional\",\"startup\":{\"mode\":\"safety\",\"configured\":true}},\"uniqueid\":\"00:17:88:01:02:ac:f5:5a-0b\",\"swversion\":\"5.130.1.30000\"},\"4\":{\"state\":{\"on\":false,\"bri\":157,\"ct\":309,\"alert\":\"none\",\"colormode\":\"ct\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"notupdatable\",\"lastinstall\":null},\"type\":\"Color temperature light\",\"name\":\"Esszimmer 1\",\"modelid\":\"TRADFRI bulb GU10 WS 400lm\",\"manufacturername\":\"IKEA of Sweden\",\"productname\":\"Color temperature light\",\"capabilities\":{\"certified\":false,\"control\":{\"ct\":{\"min\":250,\"max\":454}},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\"},\"uniqueid\":\"00:0b:57:ff:fe:a0:d5:47-01\",\"swversion\":\"1.2.217\"},\"5\":{\"state\":{\"on\":false,\"bri\":254,\"ct\":327,\"alert\":\"none\",\"colormode\":\"ct\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"notupdatable\",\"lastinstall\":null},\"type\":\"Color temperature light\",\"name\":\"Esszimmer 2\",\"modelid\":\"TRADFRI bulb GU10 WS 400lm\",\"manufacturername\":\"IKEA of Sweden\",\"productname\":\"Color temperature light\",\"capabilities\":{\"certified\":false,\"control\":{\"ct\":{\"min\":250,\"max\":454}},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\"},\"uniqueid\":\"00:0b:57:ff:fe:b9:dd:f5-01\",\"swversion\":\"1.2.217\"},\"6\":{\"state\":{\"on\":false,\"bri\":254,\"ct\":250,\"alert\":\"none\",\"colormode\":\"ct\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"notupdatable\",\"lastinstall\":null},\"type\":\"Color temperature light\",\"name\":\"Wohnzimmer 2\",\"modelid\":\"TRADFRI bulb GU10 WS 400lm\",\"manufacturername\":\"IKEA of Sweden\",\"productname\":\"Color temperature light\",\"capabilities\":{\"certified\":false,\"control\":{\"ct\":{\"min\":250,\"max\":454}},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\"},\"uniqueid\":\"00:0b:57:ff:fe:c6:a2:c3-01\",\"swversion\":\"1.2.217\"},\"7\":{\"state\":{\"on\":false,\"bri\":121,\"ct\":320,\"alert\":\"none\",\"colormode\":\"ct\",\"mode\":\"homeautomation\",\"reachable\":true},\"swupdate\":{\"state\":\"notupdatable\",\"lastinstall\":null},\"type\":\"Color temperature light\",\"name\":\"Wohnzimmer 1\",\"modelid\":\"TRADFRI bulb GU10 WS 400lm\",\"manufacturername\":\"IKEA of Sweden\",\"productname\":\"Color temperature light\",\"capabilities\":{\"certified\":false,\"control\":{\"ct\":{\"min\":250,\"max\":454}},\"streaming\":{\"renderer\":false,\"proxy\":false}},\"config\":{\"archetype\":\"classicbulb\",\"function\":\"functional\",\"direction\":\"omnidirectional\"},\"uniqueid\":\"00:0b:57:ff:fe:9a:d9:53-01\",\"swversion\":\"1.2.217\"}}";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(response, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<HueOptions>>();
			var hueOptions = new HueOptions()
			{
				BridgeIP = "192.168.0.5",
				BridgePort = 0,
				UserKey = "abc123"
			};
			optionsMonitor.CurrentValue.Returns(hueOptions);
			var hueProvider = new HueProvider(clientFactory, optionsMonitor);

			var lights = await hueProvider.GetAllLightsAsync();

			Assert.Equal(7, lights.Count);
		}
    }
}
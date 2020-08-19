using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Sensors;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.Sensors;
using HomeAPI.Backend.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Providers.Sensors
{
	public class HueSensorProviderTests
	{
		#region HueSensorProvider

		[Fact]
		public void HueSensorProvider_Constructor()
		{
			var clientFactory = Substitute.For<IHttpClientFactory>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<HueOptions>>();
			var hueOptions = new HueOptions()
			{
				BridgeIP = "192.168.0.5",
				BridgePort = 4100,
				UserKey = "abc123"
			};
			optionsMonitor.CurrentValue.Returns(hueOptions);

			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			Assert.NotNull(hueSensorProvider); // dummy test
		}

		#endregion

		#region GetAllSensorsAsync

		[Fact]
		public async Task GetAllSensorsAsync_Successful()
		{
			string response = "{\"1\":{\"state\":{\"daylight\":true,\"lastupdated\":\"2020-08-19T04:52:00\"},\"config\":{\"on\":true,\"configured\":true,\"sunriseoffset\":30,\"sunsetoffset\":-30},\"name\":\"Daylight\",\"type\":\"Daylight\",\"modelid\":\"PHDL00\",\"manufacturername\":\"Signify Netherlands B.V.\",\"swversion\":\"1.0\"},\"5\":{\"state\":{\"status\":0,\"lastupdated\":\"2019-04-05T05:25:00\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"Licht-Status\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"Model 2015\",\"manufacturername\":\"all 4 hue\",\"swversion\":\"1.0\",\"uniqueid\":\"PFHS-LIGHT-STATE\",\"recycle\":false},\"6\":{\"state\":{\"temperature\":2382,\"lastupdated\":\"2020-08-19T10:28:19\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"7\":{\"state\":{\"presence\":false,\"lastupdated\":\"2020-08-19T09:38:46\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"sensitivity\":2,\"sensitivitymax\":2,\"pending\":[]},\"name\":\"Flur Sensor\",\"type\":\"ZLLPresence\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue motion sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0406\",\"capabilities\":{\"certified\":true,\"primary\":true}},\"8\":{\"state\":{\"lightlevel\":4225,\"dark\":true,\"daylight\":false,\"lastupdated\":\"2020-08-19T10:28:41\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"tholddark\":16000,\"tholdoffset\":7000,\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue ambient light sensor 1\",\"type\":\"ZLLLightLevel\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue ambient light sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0400\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"9\":{\"state\":{\"status\":0,\"lastupdated\":\"2020-08-19T09:39:31\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"MotionSensor 7.Companion\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"PHA_STATE\",\"manufacturername\":\"Philips\",\"swversion\":\"1.0\",\"uniqueid\":\"MotionSensor 7.Companion\",\"recycle\":true}}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllSensorsAsync();

			Assert.Equal(6, sensors.Count);
		}

		[Fact]
		public async Task GetAllSensorsAsync_InvalidJson()
		{
			string response = "{abc}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllSensorsAsync();

			Assert.Empty(sensors);
		}

		[Fact]
		public async Task GetAllSensorsAsync_HttpError()
		{
			string response = "{\"1\":{\"state\":{\"daylight\":true,\"lastupdated\":\"2020-08-19T04:52:00\"},\"config\":{\"on\":true,\"configured\":true,\"sunriseoffset\":30,\"sunsetoffset\":-30},\"name\":\"Daylight\",\"type\":\"Daylight\",\"modelid\":\"PHDL00\",\"manufacturername\":\"Signify Netherlands B.V.\",\"swversion\":\"1.0\"},\"5\":{\"state\":{\"status\":0,\"lastupdated\":\"2019-04-05T05:25:00\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"Licht-Status\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"Model 2015\",\"manufacturername\":\"all 4 hue\",\"swversion\":\"1.0\",\"uniqueid\":\"PFHS-LIGHT-STATE\",\"recycle\":false},\"6\":{\"state\":{\"temperature\":2382,\"lastupdated\":\"2020-08-19T10:28:19\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"7\":{\"state\":{\"presence\":false,\"lastupdated\":\"2020-08-19T09:38:46\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"sensitivity\":2,\"sensitivitymax\":2,\"pending\":[]},\"name\":\"Flur Sensor\",\"type\":\"ZLLPresence\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue motion sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0406\",\"capabilities\":{\"certified\":true,\"primary\":true}},\"8\":{\"state\":{\"lightlevel\":4225,\"dark\":true,\"daylight\":false,\"lastupdated\":\"2020-08-19T10:28:41\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"tholddark\":16000,\"tholdoffset\":7000,\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue ambient light sensor 1\",\"type\":\"ZLLLightLevel\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue ambient light sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0400\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"9\":{\"state\":{\"status\":0,\"lastupdated\":\"2020-08-19T09:39:31\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"MotionSensor 7.Companion\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"PHA_STATE\",\"manufacturername\":\"Philips\",\"swversion\":\"1.0\",\"uniqueid\":\"MotionSensor 7.Companion\",\"recycle\":true}}";
			HttpStatusCode statusCode = HttpStatusCode.NotFound;
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllSensorsAsync();

			Assert.Empty(sensors);
		}

		#endregion

		#region GetSensorByIdAsync

		[Fact]
		public async Task GetSensorByIdAsync_Successful()
		{
			string response = "{\"state\":{\"temperature\":2354,\"lastupdated\":\"2020-08-19T10:43:17\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(6);

			Assert.NotNull(sensor);
			Assert.Equal(6, sensor.Id);
			Assert.Equal(SensorType.Temperature, sensor.Type);
			Assert.Equal(60, sensor.BatteryPercentage);
			Assert.Equal(23.54f, (sensor as TemperatureSensor).State.Temperature);
		}

		[Fact]
		public async Task GetSensorByIdAsync_IdNotExisting()
		{
			string response = "[{\"error\":{\"type\":3,\"address\":\"/sensors/60\",\"description\":\"resource, /sensors/60, not available\"}}]";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(60);

			Assert.Null(sensor);
		}

		[Fact]
		public async Task GetSensorByIdAsync_InvalidJson()
		{
			string response = "{abc}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(6);

			Assert.Null(sensor);
		}

		[Fact]
		public async Task GetSensorByIdAsync_HttpError()
		{
			string response = "{\"state\":{\"temperature\":2354,\"lastupdated\":\"2020-08-19T10:43:17\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}}";
			HttpStatusCode statusCode = HttpStatusCode.NotFound;
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(6);

			Assert.Null(sensor);
		}

		#endregion

		#region GetAllTemperatureSensorsAsync

		[Fact]
		public async Task GetAllTemperatureSensorsAsync_Successful()
		{
			string response = "{\"1\":{\"state\":{\"daylight\":true,\"lastupdated\":\"2020-08-19T04:52:00\"},\"config\":{\"on\":true,\"configured\":true,\"sunriseoffset\":30,\"sunsetoffset\":-30},\"name\":\"Daylight\",\"type\":\"Daylight\",\"modelid\":\"PHDL00\",\"manufacturername\":\"Signify Netherlands B.V.\",\"swversion\":\"1.0\"},\"5\":{\"state\":{\"status\":0,\"lastupdated\":\"2019-04-05T05:25:00\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"Licht-Status\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"Model 2015\",\"manufacturername\":\"all 4 hue\",\"swversion\":\"1.0\",\"uniqueid\":\"PFHS-LIGHT-STATE\",\"recycle\":false},\"6\":{\"state\":{\"temperature\":2382,\"lastupdated\":\"2020-08-19T10:28:19\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"7\":{\"state\":{\"presence\":false,\"lastupdated\":\"2020-08-19T09:38:46\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"sensitivity\":2,\"sensitivitymax\":2,\"pending\":[]},\"name\":\"Flur Sensor\",\"type\":\"ZLLPresence\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue motion sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0406\",\"capabilities\":{\"certified\":true,\"primary\":true}},\"8\":{\"state\":{\"lightlevel\":4225,\"dark\":true,\"daylight\":false,\"lastupdated\":\"2020-08-19T10:28:41\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"tholddark\":16000,\"tholdoffset\":7000,\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue ambient light sensor 1\",\"type\":\"ZLLLightLevel\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue ambient light sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0400\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"9\":{\"state\":{\"status\":0,\"lastupdated\":\"2020-08-19T09:39:31\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"MotionSensor 7.Companion\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"PHA_STATE\",\"manufacturername\":\"Philips\",\"swversion\":\"1.0\",\"uniqueid\":\"MotionSensor 7.Companion\",\"recycle\":true}}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllTemperatureSensorsAsync();

			Assert.Single(sensors);
			Assert.IsType<TemperatureSensor>(sensors[0]);
		}

		[Fact]
		public async Task GetAllTemperatureSensorsAsync_InvalidJson()
		{
			string response = "{abc}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllTemperatureSensorsAsync();

			Assert.Empty(sensors);
		}

		[Fact]
		public async Task GetAllTemperatureSensorsAsync_HttpError()
		{
			string response = "{\"1\":{\"state\":{\"daylight\":true,\"lastupdated\":\"2020-08-19T04:52:00\"},\"config\":{\"on\":true,\"configured\":true,\"sunriseoffset\":30,\"sunsetoffset\":-30},\"name\":\"Daylight\",\"type\":\"Daylight\",\"modelid\":\"PHDL00\",\"manufacturername\":\"Signify Netherlands B.V.\",\"swversion\":\"1.0\"},\"5\":{\"state\":{\"status\":0,\"lastupdated\":\"2019-04-05T05:25:00\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"Licht-Status\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"Model 2015\",\"manufacturername\":\"all 4 hue\",\"swversion\":\"1.0\",\"uniqueid\":\"PFHS-LIGHT-STATE\",\"recycle\":false},\"6\":{\"state\":{\"temperature\":2382,\"lastupdated\":\"2020-08-19T10:28:19\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"7\":{\"state\":{\"presence\":false,\"lastupdated\":\"2020-08-19T09:38:46\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"sensitivity\":2,\"sensitivitymax\":2,\"pending\":[]},\"name\":\"Flur Sensor\",\"type\":\"ZLLPresence\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue motion sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0406\",\"capabilities\":{\"certified\":true,\"primary\":true}},\"8\":{\"state\":{\"lightlevel\":4225,\"dark\":true,\"daylight\":false,\"lastupdated\":\"2020-08-19T10:28:41\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"tholddark\":16000,\"tholdoffset\":7000,\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue ambient light sensor 1\",\"type\":\"ZLLLightLevel\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue ambient light sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0400\",\"capabilities\":{\"certified\":true,\"primary\":false}},\"9\":{\"state\":{\"status\":0,\"lastupdated\":\"2020-08-19T09:39:31\"},\"config\":{\"on\":true,\"reachable\":true},\"name\":\"MotionSensor 7.Companion\",\"type\":\"CLIPGenericStatus\",\"modelid\":\"PHA_STATE\",\"manufacturername\":\"Philips\",\"swversion\":\"1.0\",\"uniqueid\":\"MotionSensor 7.Companion\",\"recycle\":true}}";
			HttpStatusCode statusCode = HttpStatusCode.NotFound;
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensors = await hueSensorProvider.GetAllTemperatureSensorsAsync();

			Assert.Empty(sensors);
		}

		#endregion

		#region GetTemperatureSensorByIdAsync

		[Fact]
		public async Task GetTemperatureSensorByIdAsync_Successful()
		{
			string response = "{\"state\":{\"temperature\":2354,\"lastupdated\":\"2020-08-19T10:43:17\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetTemperatureSensorByIdAsync(6);

			Assert.NotNull(sensor);
			Assert.Equal(6, sensor.Id);
			Assert.Equal(SensorType.Temperature, sensor.Type);
			Assert.Equal(60, sensor.BatteryPercentage);
			Assert.Equal(23.54f, sensor.State.Temperature);
		}

		[Fact]
		public async Task GetTemperatureSensorByIdAsync_IdNotExisting()
		{
			string response = "[{\"error\":{\"type\":3,\"address\":\"/sensors/60\",\"description\":\"resource, /sensors/60, not available\"}}]";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetTemperatureSensorByIdAsync(60);

			Assert.Null(sensor);
		}

		[Fact]
		public async Task GetTemperatureSensorByIdAsync_InvalidJson()
		{
			string response = "{abc}";
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(6);

			Assert.Null(sensor);
		}

		[Fact]
		public async Task GetTemperatureSensorByIdAsync_HttpError()
		{
			string response = "{\"state\":{\"temperature\":2354,\"lastupdated\":\"2020-08-19T10:43:17\"},\"swupdate\":{\"state\":\"noupdates\",\"lastinstall\":\"2019-03-12T13:27:46\"},\"config\":{\"on\":true,\"battery\":60,\"reachable\":true,\"alert\":\"none\",\"ledindication\":false,\"usertest\":false,\"pending\":[]},\"name\":\"Hue temperature sensor 1\",\"type\":\"ZLLTemperature\",\"modelid\":\"SML001\",\"manufacturername\":\"Signify Netherlands B.V.\",\"productname\":\"Hue temperature sensor\",\"swversion\":\"6.1.1.27575\",\"uniqueid\":\"00:17:88:01:03:28:3e:d2-02-0402\",\"capabilities\":{\"certified\":true,\"primary\":false}}";
			HttpStatusCode statusCode = HttpStatusCode.NotFound;
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
			var hueSensorProvider = new HueSensorProvider(clientFactory, optionsMonitor);

			var sensor = await hueSensorProvider.GetSensorByIdAsync(6);

			Assert.Null(sensor);
		}

		#endregion
	}
}
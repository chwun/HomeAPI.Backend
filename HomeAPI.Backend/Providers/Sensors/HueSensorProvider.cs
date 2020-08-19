using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Sensors;
using HomeAPI.Backend.Models.Sensors.Hue;
using HomeAPI.Backend.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HomeAPI.Backend.Providers.Sensors
{
	public class HueSensorProvider : HueProvider, IHueSensorProvider
	{
		public HueSensorProvider(IHttpClientFactory clientFactory, IOptionsMonitor<HueOptions> optionsMonitor)
			: base(clientFactory, optionsMonitor)
		{
		}

		public async Task<List<Sensor>> GetAllSensorsAsync()
		{
			string url = $"{apiUrl}/sensors";

			List<Sensor> result = new List<Sensor>();

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);
				var sensorsDict = JsonConvert.DeserializeObject<Dictionary<int, HueSensor>>(jsonText);

				foreach (KeyValuePair<int, HueSensor> keyValue in sensorsDict)
				{
					Sensor sensor = keyValue.Value.ToSensor(keyValue.Key);
					result.Add(sensor);
				}
			}
			catch (Exception)
			{
			}

			return result.OrderBy(x => x.Id).ToList();
		}

		public async Task<Sensor> GetSensorByIdAsync(int sensorId)
		{
			string url = $"{apiUrl}/sensors/{sensorId}";

			Sensor result = null;

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);

				var hueSensor = JsonConvert.DeserializeObject<HueSensor>(jsonText);

				result = hueSensor.ToSensor(sensorId);
			}
			catch (Exception)
			{
			}

			return result;
		}

		public async Task<List<TemperatureSensor>> GetAllTemperatureSensorsAsync()
		{
			string url = $"{apiUrl}/sensors";

			List<TemperatureSensor> result = new List<TemperatureSensor>();

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);
				var sensorsDict = JsonConvert.DeserializeObject<Dictionary<int, HueSensor>>(jsonText);

				foreach (KeyValuePair<int, HueSensor> keyValue in sensorsDict)
				{
					Sensor sensor = keyValue.Value.ToSensor(keyValue.Key);

					if ((sensor is TemperatureSensor temperatureSensor) && (sensor.Type == SensorType.Temperature))
					{
						result.Add(temperatureSensor);
					}
				}
			}
			catch (Exception)
			{
			}

			return result.OrderBy(x => x.Id).ToList();
		}

		public async Task<TemperatureSensor> GetTemperatureSensorByIdAsync(int sensorId)
		{
			string url = $"{apiUrl}/sensors/{sensorId}";

			TemperatureSensor result = null;

			try
			{
				var httpClient = clientFactory.CreateClient();
				var jsonText = await httpClient.GetStringAsync(url);

				var hueSensor = JsonConvert.DeserializeObject<HueSensor>(jsonText);

				result = hueSensor.ToSensor(sensorId) as TemperatureSensor;
			}
			catch (Exception)
			{
			}

			return result;
		}
	}
}
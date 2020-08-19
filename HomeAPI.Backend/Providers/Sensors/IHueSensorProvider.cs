using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Sensors;

namespace HomeAPI.Backend.Providers.Sensors
{
	public interface IHueSensorProvider
	{
		Task<List<Sensor>> GetAllSensorsAsync();

		Task<Sensor> GetSensorByIdAsync(int sensorId);

		Task<List<TemperatureSensor>> GetAllTemperatureSensorsAsync();

		Task<TemperatureSensor> GetTemperatureSensorByIdAsync(int sensorId);
	}
}
using System;
using Newtonsoft.Json;

namespace HomeAPI.Backend.Models.Sensors.Hue
{
	public class HueSensorState
	{
		[JsonProperty("temperature")]
		public int Temperature { get; set; }

		[JsonProperty("lastupdated")]
		public DateTime LastUpdated { get; set; }
	}
}
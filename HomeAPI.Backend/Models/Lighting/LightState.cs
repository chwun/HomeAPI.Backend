namespace HomeAPI.Backend.Models.Lighting
{
	public class LightState
	{
		public bool On { get; set; }

		public int Brightness { get; set; }

		public int Saturation { get; set; }

		public int ColorTemperature { get; set; }

		public bool Reachable { get; set; }
	}
}
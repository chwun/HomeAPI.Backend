namespace HomeAPI.Backend.Models.Lighting
{
	public class LightStateUpdate
	{
		public bool On { get; set; }

		public int Brightness { get; set; }

		public int Saturation { get; set; }

		public int Hue { get; set; }

		public int ColorTemperature { get; set; }
	}
}
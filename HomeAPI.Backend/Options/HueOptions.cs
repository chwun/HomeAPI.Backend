namespace HomeAPI.Backend.Options
{
	public class HueOptions
	{
		public string BridgeIP { get; set; }

		public int BridgePort { get; set; }

		public string UserKey { get; set; }

		public HueOptions()
		{
		}
	}
}
namespace HomeAPI.Backend.Options
{
	public class OWMOptions
	{
		public string ApiKey { get; set; }

		public float Latitude { get; set; }

		public float Longitude { get; set; }

		public string LanguageCode { get; set; }
	}
}
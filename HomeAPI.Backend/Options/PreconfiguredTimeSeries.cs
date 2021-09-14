using System.Collections.Generic;

namespace HomeAPI.Backend.Options
{
	public class PreconfiguredTimeSeries
	{
		public List<PreconfiguredTimeSeriesElement> Elements { get; set; } = new();
	}

	public class PreconfiguredTimeSeriesElement
	{
		public string DisplayName { get; set; }

		public string MeasurementName { get; set; }

		public string MeasurementLocation { get; set; }
	}
}
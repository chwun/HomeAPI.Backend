using System.Collections.Generic;

namespace HomeAPI.Backend.Models.TimeSeries
{
	public class TimeSeriesRequest
	{
		public string MeasurementName { get; set; }

		public TimeSeriesValueType ValueType { get; set; }

		public Dictionary<string, string> Tags { get; set; }

		public TimeSeriesRange Range { get; set; }

		public TimeSeriesRequest()
		{
		}
	}
}
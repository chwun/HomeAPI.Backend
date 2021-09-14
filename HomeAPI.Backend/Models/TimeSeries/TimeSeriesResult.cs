using System.Collections.Generic;

namespace HomeAPI.Backend.Models.TimeSeries
{
	public class TimeSeriesResult
	{
		public string DisplayName { get; set; }

		public List<DataPoint> DataPoints { get; set; }
	}
}
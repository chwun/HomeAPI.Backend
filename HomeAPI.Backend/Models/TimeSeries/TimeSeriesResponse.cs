using System.Collections.Generic;

namespace HomeAPI.Backend.Models.TimeSeries
{
	public class TimeSeriesResponse
	{
		public TimeSeriesResponseStatus Status { get; set; }

		public List<DataPoint> DataPoints { get; set; }

		public TimeSeriesResponse()
		{
		}
	}
}
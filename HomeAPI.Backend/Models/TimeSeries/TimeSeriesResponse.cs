using System.Collections.Generic;

namespace HomeAPI.Backend.Models.TimeSeries
{
	public class TimeSeriesResponse
	{
		public TimeSeriesResponseStatus Status { get; set; }

		public TimeSeriesResult TimeSeriesResult { get; set; }
	}
}
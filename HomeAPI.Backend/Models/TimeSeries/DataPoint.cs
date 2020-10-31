using System;

namespace HomeAPI.Backend.Models.TimeSeries
{
    public abstract class DataPoint
    {
        public DateTime Timestamp {get;set;}

		protected DataPoint(DateTime timestamp)
		{
			Timestamp = timestamp;
		}
    }
}
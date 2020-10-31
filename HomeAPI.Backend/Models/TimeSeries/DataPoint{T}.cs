using System;

namespace HomeAPI.Backend.Models.TimeSeries
{
	public class DataPoint<T> : DataPoint
	{
		public T Value { get; set; }

		public DataPoint(DateTime timestamp, T value)
			: base(timestamp)
		{
			Value = value;
		}
	}
}
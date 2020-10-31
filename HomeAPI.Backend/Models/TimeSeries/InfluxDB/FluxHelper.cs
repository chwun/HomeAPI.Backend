using System.Collections.Generic;
using System.Text;

namespace HomeAPI.Backend.Models.TimeSeries.InfluxDB
{
	public class FluxHelper : IFluxHelper
	{
		public string CreateQuery(string database, TimeSeriesRequest request)
		{
			string rangeString = CreateRangeString(request.Range);
			string filterString = CreateFilterString(request.MeasurementName, request.Tags);

			return $"from(bucket: \"{database}\") {rangeString} {filterString}";
		}

		private string CreateRangeString(TimeSeriesRange range)
		{
			string duration = range switch
			{
				TimeSeriesRange.OneDay => "-1d",
				TimeSeriesRange.OneWeek => "-1w",
				TimeSeriesRange.OneMonth => "-1mo",
				TimeSeriesRange.OneYear => "-1y",
				_ => "1d"
			};

			return $"|> range(start: {duration})";
		}

		private string CreateFilterString(string measurementName, Dictionary<string, string> tags)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("|> filter(fn: (r) => r._measurement == \"");
			sb.Append(measurementName);
			sb.Append("\"");

			foreach (var tag in tags)
			{
				sb.Append($" and r.{tag.Key} == \"{tag.Value}\"");
			}

			sb.Append(")");

			return sb.ToString();
		}
	}
}
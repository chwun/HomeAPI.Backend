using System.Collections.Generic;
using HomeAPI.Backend.Models.TimeSeries;
using HomeAPI.Backend.Models.TimeSeries.InfluxDB;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.TimeSeries.InfluxDB
{
	public class FluxHelperTests
	{
		#region CreateQuery

		[Fact]
		public void CreateQuery()
		{
			string database = "TestDB";
			TimeSeriesRequest request = new TimeSeriesRequest()
			{
				MeasurementName = "temperature",
				ValueType = TimeSeriesValueType.Float,
				Range = TimeSeriesRange.OneDay,
				Tags = new Dictionary<string, string>()
				{
					["location"] = "Room1"
				}
			};
			FluxHelper fluxHelper = new FluxHelper();

			var result = fluxHelper.CreateQuery(database, request);

			Assert.Equal("from(bucket: \"TestDB\") |> range(start: -1d) |> filter(fn: (r) => r._measurement == \"temperature\" and r.location == \"Room1\")", result);
		}

		#endregion
	}
}
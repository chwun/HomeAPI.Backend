using System.Collections.Generic;

namespace HomeAPI.Backend.Models.TimeSeries.InfluxDB
{
	public interface IInfluxDBQueryResultParser
	{
		List<DataPoint> ParseQueryResult(string queryResult, TimeSeriesValueType valueType);
	}
}
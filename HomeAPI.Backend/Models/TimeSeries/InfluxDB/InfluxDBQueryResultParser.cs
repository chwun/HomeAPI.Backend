using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HomeAPI.Backend.Models.TimeSeries.InfluxDB
{
	public class InfluxDBQueryResultParser : IInfluxDBQueryResultParser
	{
		public List<DataPoint> ParseQueryResult(string queryResult, TimeSeriesValueType valueType)
		{
			List<DataPoint> dataPoints = new List<DataPoint>();

			List<string> lines = SplitIntoLines(queryResult);

			int indexTime = GetCsvIndex("_time", lines[0]);
			int indexValue = GetCsvIndex("_value", lines[0]);

			for (int i = 1; i < lines.Count; i++)
			{
				var csvFields = SplitCsv(lines[i]);

				DateTime timestamp = DateTime.Parse(csvFields[indexTime]);
				string value = csvFields[indexValue];

				DataPoint dataPoint = CreateDataPoint(valueType, timestamp, value);
				dataPoints.Add(dataPoint);
			}

			return dataPoints;
		}

		private List<string> SplitIntoLines(string queryResult)
		{
			return queryResult.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries).Skip(3).ToList();
		}

		private int GetCsvIndex(string fieldName, string line)
		{
			List<string> headers = SplitCsv(line);

			return headers.FindIndex(x => x.Equals(fieldName));
		}

		private List<string> SplitCsv(string line)
		{
			return line.Split(',', StringSplitOptions.None).ToList();
		}

		private DataPoint CreateDataPoint(TimeSeriesValueType valueType, DateTime timestamp, string value)
		{
			DataPoint dataPoint;

			switch (valueType)
			{
				case TimeSeriesValueType.Float:
					float floatValue = float.Parse(value, CultureInfo.InvariantCulture);
					dataPoint = new DataPoint<float>(timestamp, floatValue);
					break;

				default:
					throw new NotImplementedException($"value type {valueType} not implemented!");
			}

			return dataPoint;
		}
	}
}
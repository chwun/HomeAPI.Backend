using System.Threading.Tasks;
using HomeAPI.Backend.Models.TimeSeries;

namespace HomeAPI.Backend.Providers.TimeSeries
{
	public interface IInfluxDBProvider
	{
		Task<TimeSeriesResponse> GetTimeSeriesAsync(TimeSeriesRequest request, string displayName);
	}
}
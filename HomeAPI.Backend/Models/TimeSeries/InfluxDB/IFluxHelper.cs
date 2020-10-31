namespace HomeAPI.Backend.Models.TimeSeries.InfluxDB
{
    public interface IFluxHelper
    {
         string CreateQuery(string database, TimeSeriesRequest request);
    }
}
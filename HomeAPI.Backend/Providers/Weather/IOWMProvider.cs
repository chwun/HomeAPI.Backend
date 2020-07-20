using System.Threading.Tasks;
using HomeAPI.Backend.Models.Weather;

namespace HomeAPI.Backend.Providers.Weather
{
    public interface IOWMProvider
    {
         Task<CurrentWeatherResponse> GetWeatherAsync();
    }
}
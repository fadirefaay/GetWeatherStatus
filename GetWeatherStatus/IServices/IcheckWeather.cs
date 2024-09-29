using GetWeatherStatus.DTO;
using GetWeatherStatus.Models;

namespace GetWeatherStatus.IServices
{
    public interface IcheckWeather
    {
         Task<ApiResponse> GetNearestCityAirQuality(RequestDTO requestDTO);
         Task<AirQuality> getlastdata();
         Task CheckAndSaveAirQuality();
    }
}

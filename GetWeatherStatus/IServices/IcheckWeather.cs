using GetWeatherStatus.DTO;
using GetWeatherStatus.Models;
using System.Collections.Generic;

namespace GetWeatherStatus.IServices
{
    public interface IcheckWeather
    {
         Task<ApiResponse> GetNearestCityAirQuality(RequestDTO requestDTO);
         Task<AirQuality> getlastdata();
         Task CheckAndSaveAirQuality();
         Task<IEnumerable<AirQuality>> GetAllCountiesWeatherStatus();
    }
}

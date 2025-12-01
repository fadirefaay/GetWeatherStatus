using GetWeatherStatus.DTO;
using GetWeatherStatus.IServices;
using GetWeatherStatus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GetWeatherStatus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IcheckWeather weather;

        public WeatherController(IcheckWeather weather)
        {
            this.weather = weather;
        }

        [HttpGet("nearest_city")]
        public async Task<IActionResult> getWeather([FromQuery] RequestDTO requestDTO)
        {

            try
            {
                var result = await weather.GetNearestCityAirQuality(requestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("parismostpolluted")]
        public async Task<IActionResult> GetMostPollutedTime()
        {
            var mostPolluted = await weather.getlastdata();

            if (mostPolluted == null)
            {
                return NotFound("No data available for Paris.");
            }

            return Ok(new { mostPolluted.Timestamp });
        }

        [HttpGet("counties")]
        public async Task<IActionResult> GetAllCountiesWeatherStatus()
        {
            try
            {
                IEnumerable<AirQuality> countiesWeather = await weather.GetAllCountiesWeatherStatus();

                if (!countiesWeather.Any())
                {
                    return NotFound("No air quality data available.");
                }

                return Ok(countiesWeather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

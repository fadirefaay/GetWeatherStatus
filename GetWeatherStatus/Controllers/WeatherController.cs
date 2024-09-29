using GetWeatherStatus.DTO;
using GetWeatherStatus.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var result =await weather.GetNearestCityAirQuality(requestDTO);
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("parismostpolluted")]
        public IActionResult GetMostPollutedTime()
        {
            var mostPolluted = weather.getlastdata().Result;

            if (mostPolluted == null)
            {
                return NotFound("No data available for Paris.");
            }

            return Ok(new { mostPolluted.Timestamp });
        }







    }
}

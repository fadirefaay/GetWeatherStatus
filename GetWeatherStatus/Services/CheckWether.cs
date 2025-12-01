using GetWeatherStatus.DTO;
using GetWeatherStatus.IServices;
using GetWeatherStatus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GetWeatherStatus.Services
{
    public class CheckWether : IcheckWeather
    {
        private readonly HttpClient httpClient;
        private readonly weatherDb db;
        private readonly Keys appSettings;

        public CheckWether(HttpClient httpClient, IOptions<Keys> appSettings, weatherDb db)
        {
            this.httpClient = httpClient;
            this.db = db;
            this.appSettings = appSettings.Value;
        }

        public async Task<ApiResponse> GetNearestCityAirQuality(RequestDTO requestDTO)
        {
            httpClient.BaseAddress = new Uri(appSettings.BaseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string x = $"nearest_city?lat={requestDTO.latitude}&lon={requestDTO.longitude}&key={appSettings.Key}";
            HttpResponseMessage Res = await httpClient.GetAsync(x);
            if (Res.IsSuccessStatusCode)
            {
                var EmpResponse = await Res.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(EmpResponse);
                return apiResponse;
            }
            throw new Exception("Unable to fetch air quality data.");

        }

        public async Task<IEnumerable<AirQuality>> GetAllCountiesWeatherStatus()
        {
            return await db.AirQualityRecords
                .AsNoTracking()
                .OrderBy(record => record.Location)
                .ThenByDescending(record => record.Timestamp)
                .ToListAsync();
        }

        public async Task CheckAndSaveAirQuality()
        {
            RequestDTO request = new RequestDTO()
            {
                latitude = "48.856613",
                longitude = "2.352222"

            };


            var airQuality = await GetNearestCityAirQuality(request);

            if (airQuality != null)
            {
                var AirQuality = new AirQuality
                {
                    Location = "Paris",
                    aqius = airQuality.Data.current.pollution.aqius,
                    Timestamp = DateTime.UtcNow
                };

                db.AirQualityRecords.Add(AirQuality);
                await db.SaveChangesAsync();
            }
        }

        public async Task<AirQuality> getlastdata()
        {
            return await db.AirQualityRecords
                .Where(r => r.Location == "Paris")
                .OrderByDescending(r => r.aqius) // Assuming Data contains pollution level
                .FirstOrDefaultAsync();
        }

    }
}

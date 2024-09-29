using GetWeatherStatus.IServices;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using Microsoft.Extensions.Options;
using GetWeatherStatus.DTO;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using GetWeatherStatus.Models;

namespace GetWeatherStatus.Services
{
    public class CheckWether : IcheckWeather
    {
        private readonly HttpClient httpClient;
        private readonly weatherDb db;
        private readonly Keys appSettings;
        private readonly string _apiKey = "c2371406-517c-4c54-8fbc-6b921fa97b3e";

        public CheckWether(HttpClient httpClient, IOptions<Keys> appSettings,weatherDb db)
        {
            this.httpClient = httpClient;
            this.db = db;
            this.appSettings = appSettings.Value;
        }
        public async Task<ApiResponse> GetNearestCityAirQuality(RequestDTO requestDTO)
        {
            
            Data data = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(appSettings.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string x = $"nearest_city?lat={requestDTO.latitude}&lon={requestDTO.longitude}&key={appSettings.Key}";
                HttpResponseMessage Res = await client.GetAsync(x);
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //data = JsonConvert.DeserializeObject<Data>(EmpResponse);
                    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(EmpResponse);
                    return apiResponse;
                }
                throw new Exception();
            }

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
          return  db.AirQualityRecords
                .Where(r => r.Location == "Paris")
                .OrderByDescending(r => r.aqius) // Assuming Data contains pollution level
                .FirstOrDefault();
        }

    }
}

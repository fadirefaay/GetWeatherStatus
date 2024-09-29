namespace GetWeatherStatus.Models
{
    public class AirQuality
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int aqius { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}

using Microsoft.EntityFrameworkCore;

namespace GetWeatherStatus.Models
{
    public class weatherDb : DbContext
    {
        public DbSet<AirQuality> AirQualityRecords { get; set; }

        public weatherDb(DbContextOptions<weatherDb> options) : base(options)
        {
        }
    }
}

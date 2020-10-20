namespace CarRentalSystem.Statistics.Data
{
    using System.Linq;
    using CarRentalSystem.Services.Data;
    using Microsoft.Extensions.Options;
    using Models;

    public class StatisticsDataSeeder : IDataSeeder
    {
        private readonly StatisticsDbContext db;
        private readonly ApplicationSettings applicationSettings;

        public StatisticsDataSeeder(
            StatisticsDbContext db, 
            IOptions<ApplicationSettings> applicationSettings)
        {
            this.db = db;
            this.applicationSettings = applicationSettings.Value;
        }

        public void SeedData()
        {
            if (!this.db.Statistics.Any())
            {
                this.db.Statistics.Add(new Statistics
                {
                    TotalCarAds = 0,
                    TotalRentedCars = 0
                });

                this.db.SaveChanges();
            }

            if (this.applicationSettings.SeedInitialData)
            {
                var statistics = this.db.Statistics.Single();

                if (statistics.TotalCarAds == 0)
                {
                    statistics.TotalCarAds = 3;
                }

                this.db.SaveChanges();
            }
        }
    }
}

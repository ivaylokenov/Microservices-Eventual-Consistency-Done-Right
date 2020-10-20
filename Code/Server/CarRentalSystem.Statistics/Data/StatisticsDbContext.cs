namespace CarRentalSystem.Statistics.Data
{
    using System.Reflection;
    using CarRentalSystem.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StatisticsDbContext : MessageDbContext
    {
        public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarAdView> CarAdViews { get; set; }

        public DbSet<Statistics> Statistics { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}

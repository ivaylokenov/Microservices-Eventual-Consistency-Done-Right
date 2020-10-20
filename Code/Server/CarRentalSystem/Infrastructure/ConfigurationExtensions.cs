namespace CarRentalSystem.Infrastructure
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");

        public static string GetCronJobsConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("CronJobsConnection");
    }
}

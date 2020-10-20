namespace CarRentalSystem.Identity.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using CarRentalSystem.Data;
    using CarRentalSystem.Services.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Models;

    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationSettings applicationSettings;
        private readonly IdentitySettings identitySettings;

        public IdentityDataSeeder(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> applicationSettings,
            IOptions<IdentitySettings> identitySettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationSettings = applicationSettings.Value;
            this.identitySettings = identitySettings.Value;
        }

        public void SeedData()
        {
            if (!this.roleManager.Roles.Any())
            {
                Task
                    .Run(async () =>
                    {
                        var adminRole = new IdentityRole(Constants.AdministratorRoleName);

                        await this.roleManager.CreateAsync(adminRole);

                        var adminUser = new User
                        {
                            UserName = "admin@crs.com",
                            Email = "admin@crs.com",
                            SecurityStamp = "RandomSecurityStamp"
                        };

                        await this.userManager.CreateAsync(adminUser, this.identitySettings.AdminPassword);

                        await this.userManager.AddToRoleAsync(adminUser, Constants.AdministratorRoleName);
                    })
                    .GetAwaiter()
                    .GetResult();
            }

            if (this.applicationSettings.SeedInitialData)
            {
                Task
                    .Run(async () =>
                    {
                        if (await this.userManager.FindByIdAsync(DataSeederConstants.DefaultUserId) != null)
                        {
                            return;
                        }

                        var defaultUser = new User
                        {
                            Id = DataSeederConstants.DefaultUserId,
                            UserName = "coolcars@crs.com",
                            Email = "coolcars@crs.com"
                        };

                        await this.userManager.CreateAsync(defaultUser, DataSeederConstants.DefaultUserPassword);
                    })
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}

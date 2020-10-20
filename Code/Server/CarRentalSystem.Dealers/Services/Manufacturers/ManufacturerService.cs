namespace CarRentalSystem.Dealers.Services.Manufacturers
{
    using System.Threading.Tasks;
    using CarRentalSystem.Services.Data;
    using CarRentalSystem.Services.Messages;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ManufacturerService : DataService<Manufacturer>, IManufacturerService
    {
        public ManufacturerService(DbContext db, IPublisher publisher)
            : base(db, publisher)
        {
        }

        public async Task<Manufacturer> FindByName(string name)
            => await this
                .All()
                .FirstOrDefaultAsync(m => m.Name == name);
    }
}

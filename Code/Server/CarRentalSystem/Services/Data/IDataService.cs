namespace CarRentalSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IDataService<in TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);

        Task Save(params object[] messages);
    }
}

namespace CarRentalSystem.Services.Messages
{
    using System;
    using System.Threading.Tasks;
    using CarRentalSystem.Data;
    using Microsoft.EntityFrameworkCore;

    public class MessageService : IMessageService
    {
        private readonly MessageDbContext data;

        public MessageService(DbContext data) 
            => this.data = data as MessageDbContext 
                ?? throw new InvalidOperationException($"Messages can only be used with a {nameof(MessageDbContext)}.");

        public async Task<bool> IsDuplicated(
            object messageData, 
            string propertyFilter,
            object identifier)
        {
            var messageType = messageData.GetType();

            return await this.data
                .Messages
                .FromSqlRaw($"SELECT * FROM Messages WHERE Type = '{messageType.AssemblyQualifiedName}' AND JSON_VALUE(serializedData, '$.{propertyFilter}') = {identifier}")
                .AnyAsync();
        }
    }
}

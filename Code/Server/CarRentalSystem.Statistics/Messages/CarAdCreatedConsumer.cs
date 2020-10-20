namespace CarRentalSystem.Statistics.Messages
{
    using System.Threading.Tasks;
    using CarRentalSystem.Data.Models;
    using CarRentalSystem.Messages.Dealers;
    using CarRentalSystem.Services.Messages;
    using Data;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;

    public class CarAdCreatedConsumer : IConsumer<CarAdCreatedMessage>
    {
        private readonly StatisticsDbContext data;
        private readonly IMessageService messages;

        public CarAdCreatedConsumer(
            StatisticsDbContext data,
            IMessageService messages)
        {
            this.data = data;
            this.messages = messages;
        }

        public async Task Consume(ConsumeContext<CarAdCreatedMessage> context)
        {
            var message = context.Message;

            var isDuplicated = await this.messages.IsDuplicated(
                message,
                nameof(CarAdCreatedMessage.CarAdId),
                message.CarAdId);

            if (isDuplicated)
            {
                return;
            }

            var statistics = await this.data.Statistics.SingleOrDefaultAsync();

            statistics.TotalCarAds++;

            var dataMessage = new Message(message);

            dataMessage.MarkAsPublished();

            this.data.Messages.Add(dataMessage);

            await this.data.SaveChangesAsync();
        }
    }
}

namespace CarRentalSystem.Messages
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Models;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.Messages;

    public class MessagesHostedService : IHostedService
    {
        private readonly IRecurringJobManager recurringJob;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IPublisher publisher;

        public MessagesHostedService(
            IRecurringJobManager recurringJob,
            IServiceScopeFactory serviceScopeFactory,
            IPublisher publisher)
        {
            this.recurringJob = recurringJob;
            this.serviceScopeFactory = serviceScopeFactory;
            this.publisher = publisher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();

            var data = scope.ServiceProvider.GetService<DbContext>();

            if (!data.Database.CanConnect())
            {
                data.Database.Migrate();
            }

            this.recurringJob.AddOrUpdate(
                nameof(MessagesHostedService),
                () => this.ProcessPendingMessages(),
                "*/5 * * * * *");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public void ProcessPendingMessages()
        {
            using var scope = this.serviceScopeFactory.CreateScope();

            var data = scope.ServiceProvider.GetService<DbContext>();

            var messages = data
                .Set<Message>()
                .Where(m => !m.Published)
                .OrderBy(m => m.Id)
                .ToList();

            foreach (var message in messages)
            {
                this.publisher
                    .Publish(message.Data, message.Type)
                    .GetAwaiter()
                    .GetResult();

                message.MarkAsPublished();

                data.SaveChanges();
            }
        }
    }
}

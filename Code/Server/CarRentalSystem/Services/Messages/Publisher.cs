namespace CarRentalSystem.Services.Messages
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;

    public class Publisher : IPublisher
    {
        private const int TimeoutMilliseconds = 2000;

        private readonly IBus bus;

        public Publisher(IBus bus) => this.bus = bus;

        public Task Publish<TMessage>(TMessage message)
            => this.bus.Publish(message, GetCancellationToken());

        public Task Publish<TMessage>(TMessage message, Type messageType)
            => this.bus.Publish(message, messageType, GetCancellationToken());

        private static CancellationToken GetCancellationToken()
        {
            var timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            return cancellationTokenSource.Token;
        }
    }
}

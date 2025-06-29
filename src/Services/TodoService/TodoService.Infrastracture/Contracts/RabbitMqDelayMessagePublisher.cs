using TodoService.Application.Contracts.MessageBroker;
using MassTransit;

namespace TodoService.Infrastracture.Contracts;

public class RabbitMqDelayMessagePublisher(IPublishEndpoint 
    publishEndpoint) : IDelayMessagePublisher
{
    public async Task PublishAsync<T>(T message, TimeSpan delay, CancellationToken cancellationToken = default)
        where T : class
    {
        await publishEndpoint.Publish(message, context =>
        {
            context.Headers.Set("x-delay", (int)delay.TotalMilliseconds);
        }, cancellationToken: cancellationToken);
    }
}

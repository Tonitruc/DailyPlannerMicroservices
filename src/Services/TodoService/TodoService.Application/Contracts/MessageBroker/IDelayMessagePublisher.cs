namespace TodoService.Application.Contracts.MessageBroker;

public interface IDelayMessagePublisher
{
    Task PublishAsync<T>(T message, TimeSpan delay, CancellationToken cancellationToken)
        where T : class;
}

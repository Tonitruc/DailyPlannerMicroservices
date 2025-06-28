using MassTransit;
using Microsoft.Extensions.Logging;
using BaseBuldingsBlocks.Messaging;

namespace TodoService.Application.UseCases.Todos.Events;

public class TestEventHandler(ILogger<TestEventHandler> logger) 
    : IConsumer<TestMessage>
{
    public Task Consume(ConsumeContext<TestMessage> context)
    {
        logger.LogInformation(context.Message.ToString());

        return Task.CompletedTask;
    }
}

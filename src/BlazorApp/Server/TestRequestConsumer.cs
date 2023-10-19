using Contracts;

using MassTransit;

namespace BlazorApp;

public sealed class TestResponseConsumer(ILogger<TestResponseConsumer> logger) : IConsumer<TestResponse>
{
    public Task Consume(ConsumeContext<TestResponse> context)
    {
        logger.LogInformation($"Response was: {context.Message.Content}");

        return Task.CompletedTask;
    }
}

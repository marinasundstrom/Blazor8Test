using Contracts;

using MassTransit;

namespace Worker;

public sealed class TestRequestConsumer : IConsumer<TestRequest>
{
    public async Task Consume(ConsumeContext<TestRequest> context)
    {
        await Task.Delay(Random.Shared.Next(0, 5000));

        await context.Publish(new TestResponse {
            Content = $"Hello: {context.Message.SecretNumber}!"
        });
    }
}

namespace Worker.IntegrationTests;

using Contracts;

using MassTransit;
using MassTransit.Testing;

using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit.Abstractions;

public class WorkerIntegrationTest : IAsyncLifetime
{
    readonly ITestOutputHelper _testOutputHelper;
    private WebApplicationFactory<Program> factory;
    private HttpClient client;
    private ITestHarness harness;
    public WorkerIntegrationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public async Task InitializeAsync()
    {
        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_testOutputHelper));
                });

                builder.ConfigureServices(services =>
                {
                    services.AddMassTransitTestHarness(x =>
                    {
                        x.AddDelayedMessageScheduler();

                        x.AddConsumers(typeof(Contracts.TestRequest).Assembly);

                        x.UsingInMemory((context, cfg) =>
                        {
                            cfg.UseDelayedMessageScheduler();

                            cfg.ConfigureEndpoints(context);
                        });
                    });       
                });
            });

        client = factory.CreateClient();

        harness = factory.Services.GetTestHarness();

        await harness.Start();
    }

    public async Task DisposeAsync()
    {
;
    }

    [Fact(DisplayName = "Consumes TestRequest and publishes TestRespose")]
    public async Task ConsumesTestRequestAndPublishesTestRespose()
    {
        // Arrange

        // Act

        await harness.Bus.Publish(
            new TestRequest
            {
                SecretNumber = 42
            });

        // Assert

        Assert.True(await harness.Consumed.Any<TestRequest>());

        Assert.True(await harness.Published.Any<TestResponse>());
    }
}
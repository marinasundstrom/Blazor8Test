namespace BlazorApp.IntegrationTests;

using BlazorApp.Data;

using Contracts;

using FluentAssertions;

using MassTransit;
using MassTransit.Testing;

using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

using Testcontainers.SqlEdge;

using Xunit.Abstractions;

public class BlazorAppIntegrationTest : IAsyncLifetime
{
    readonly ITestOutputHelper _testOutputHelper;
    private WebApplicationFactory<Program> factory;
    private HttpClient client;
    private ITestHarness harness;
    private const string DbName = "blazorapp-db";
    private const string DbServerName = "test-sqlserver";

    static readonly SqlEdgeContainer _sqlEdgeContainer = new SqlEdgeBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:1.0.7")
        .WithHostname(DbServerName)
        .WithName(DbServerName)
        .Build();

    public BlazorAppIntegrationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public async Task InitializeAsync()
    {
        await _sqlEdgeContainer.StartAsync();

        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_testOutputHelper));
                });

                builder.ConfigureServices(async services =>
                {
                    var descriptor = services.Single(d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>((sp, options) =>
                    {
                        var connectionString = _sqlEdgeContainer.GetConnectionString().Replace("master", DbName);
                        options.UseSqlServer(connectionString);
                    });

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

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                        var configuration = scopedServices.GetRequiredService<IConfiguration>();

                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        try
                        {
                            await Seed.SeedData(db, configuration);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                });
            });

        client = factory.CreateClient();

        harness = factory.Services.GetTestHarness();

        await harness.Start();
    }

    public async Task DisposeAsync()
    {
        await _sqlEdgeContainer.StopAsync()
        .ConfigureAwait(false);
    }

    [Fact(DisplayName = "GetWeatherForecast")]
    public async Task GetWeatherForecast()
    {
        // Arrange

        // Act

        var result = await client.GetStringAsync("/api/weatherforecast?startDate=2023-09-22");

        // Assert

        result.Should().NotBeNull();
    }

    [Fact(DisplayName = "Consumes TestResponse")]
    public async Task ConsumesTestResponse()
    {
        // Arrange

        // Act

        await harness.Bus.Publish(
            new TestResponse
            {
                Content = "Test"
            });

        // Assert

        Assert.True(await harness.Consumed.Any<TestResponse>());
    }
}
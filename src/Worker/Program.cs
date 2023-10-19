using MassTransit;

using Serilog;

using Diagnostics;

string serviceName = "Worker";
string serviceVersion = "1.0";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "localhost";

        cfg.Host(rabbitmqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapObservability();

app.MapGet("/", () => "Hello World!");

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }

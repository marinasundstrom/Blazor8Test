using System.Security.Claims;

using BlazorApp;
using BlazorApp.Data;

using Diagnostics;
using Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MassTransit;
using Contracts;

string serviceName = "BlazorApp";
string serviceVersion = "1.0";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                        .Enrich.WithProperty("Application", serviceName)
                        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioningServices();
builder.Services.AddOpenApi(serviceName);

builder.Services.AddObservability(serviceName, serviceVersion, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSqlServer<ApplicationDbContext>(
    builder.Configuration.GetValue<string>("blazorapp-db-connectionstring")
    ?? builder.Configuration.GetConnectionString("BlazorAppDb"),
    c => c.EnableRetryOnFailure());

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddSingleton<RenderingContext, ServerRenderingContext>();

builder.Services.AddSingleton<RequestContext>();

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

app.UseStatusCodePagesWithRedirects("/error/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOpenApi();

// INFO: Disabled because of Prometheus polling with HTTP
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Client.Pages.FetchData).Assembly);

app.MapWeatherForecastEndpoints();

var versionedApi = app.NewVersionedApi("BlazorApp");

versionedApi.MapGroup("api/v{version:apiVersion}/identity")
    .MapIdentityApi<IdentityUser>()
    .WithTags("Identity")
    .HasApiVersion(1, 0);

versionedApi.MapGet("api/v{version:apiVersion}/requires-auth", (ClaimsPrincipal user) => $"Hello, {user.Identity?.Name}!").RequireAuthorization()
    .WithName("BlazorApp_RequiresAuth")
    .WithTags("BlazorApp")
    .HasApiVersion(1, 0);

versionedApi.MapPost("/api/v{version:apiVersion}/test", async (int secretNumber, IPublishEndpoint publishEndpoint) => await publishEndpoint.Publish(new TestRequest { SecretNumber = secretNumber }))
    .WithName("BlazorApp_Test")
    .WithTags("BlazorApp")
    .HasApiVersion(1, 0);

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    //await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    if (args.Contains("--seed"))
    {
        await SeedData(context, configuration, logger);
        return;
    }
}

app.Run();

static async Task SeedData(ApplicationDbContext context, IConfiguration configuration, ILogger<Program> logger)
{
    try
    {
        await Seed.SeedData(context, configuration);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the " +
            "database. Error: {Message}", ex.Message);
    }
}


// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }


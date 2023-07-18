using BlazorApp;
using BlazorApp.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddWebAssemblyComponents()
    .AddServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BlazorMovieContext>(c => c.UseInMemoryDatabase("db"));

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddSingleton<RenderingContext, ServerRenderingContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddWebAssemblyRenderMode()
    .AddServerRenderMode();

app.MapGet("/api/weatherforecast", async (DateOnly startDate, IWeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
    {
        var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
        return Results.Ok(forecasts);
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();;

app.Run();

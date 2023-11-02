using Microsoft.AspNetCore.Http.HttpResults;

namespace BlazorApp;

public static class WeatherForecastEndpoints
{
    public static WebApplication MapWeatherForecastEndpoints(this WebApplication app)
    {
        var versionedApi = app.NewVersionedApi("BlazorApp");

        versionedApi.MapGet("/api/v{version:apiVersion}/weatherforecast", async Task<Results<Ok<IEnumerable<WeatherForecast>>, BadRequest>> (DateOnly startDate, IWeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
        {
            var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
            return TypedResults.Ok(forecasts);
        })
        .WithName("WeatherForecast_GetWeatherForecast")
        .WithTags("WeatherForecast")
        .HasApiVersion(1, 0)
        .WithOpenApi();

        return app;
    }
}
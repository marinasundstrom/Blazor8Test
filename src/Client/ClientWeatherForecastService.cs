using BlazorApp;
using BlazorApp.Client;
using System.Net.Http.Json;

namespace Client;

public sealed class ClientWeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastClient _weatherForecastClient;

    public ClientWeatherForecastService(IWeatherForecastClient weatherForecastClient) 
    {
        _weatherForecastClient = weatherForecastClient;
    }

    public async Task<IEnumerable<BlazorApp.WeatherForecast>> GetWeatherForecasts(DateOnly startDate, CancellationToken cancellationToken = default) 
    {
        var forecasts = await _weatherForecastClient.GetWeatherForecastAsync(startDate.ToDateTime(TimeOnly.Parse("00:00")), cancellationToken);
        return forecasts.Select(weatherForecast => new BlazorApp.WeatherForecast {
            Date = DateOnly.FromDateTime(weatherForecast.Date.Date),
            TemperatureC = weatherForecast.TemperatureC,
            Summary = weatherForecast.Summary
        });
    }
}
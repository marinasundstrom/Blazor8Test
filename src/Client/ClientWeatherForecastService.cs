using BlazorApp;
using System.Net.Http.Json;

namespace Client;

public sealed class ClientWeatherForecastService : IWeatherForecastService
{
    private readonly HttpClient _httpClient;

    public ClientWeatherForecastService(HttpClient httpClient) 
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(DateOnly startDate) 
    {
        var startDateStr = DateOnly.FromDateTime(DateTime.Now).ToString("o");

        return await _httpClient.GetFromJsonAsync<WeatherForecast[]>($"/api/weatherforecast?startDate={startDateStr}");
    }
}
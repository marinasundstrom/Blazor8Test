using BlazorApp;
using Client;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddTransient<CookieHandler>();

builder.Services
    .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddScoped<RenderingContext, ClientRenderingContext>();

builder.Services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

builder.Services.AddHttpClient<BlazorApp.Client.IWeatherForecastClient>("WebAPI")
        .AddTypedClient<BlazorApp.Client.IWeatherForecastClient>((http, sp) => new BlazorApp.Client.WeatherForecastClient(http));

await builder.Build().RunAsync();
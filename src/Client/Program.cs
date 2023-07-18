using BlazorApp;

using Client;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => 
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Services.AddScoped<RenderingContext, ClientRenderingContext>();

builder.Services.AddScoped<IWeatherForecastService, ClientWeatherForecastService>();

await builder.Build().RunAsync();

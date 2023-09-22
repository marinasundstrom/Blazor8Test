using System.Security.Claims;

using BlazorApp;
using BlazorApp.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using  Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config => {
    config.PostProcess = document =>
    {
        document.Info.Title = "BlazorApp API";
    };

    config.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddWebAssemblyComponents()
    .AddServerComponents();

builder.Services.AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(c => c.UseInMemoryDatabase("db"));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddSingleton<RenderingContext, ServerRenderingContext>();

builder.Services.AddSingleton<RequestContext>();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/error/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml");
app.UseSwaggerUi3(p => p.DocumentPath = "/swagger/{documentName}/swagger.yaml");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddWebAssemblyRenderMode()
    .AddServerRenderMode()
    .AddAdditionalAssemblies(typeof(Client.FetchData).Assembly);

app.MapGroup("/identity")
    .MapIdentityApi<IdentityUser>()
    .WithTags("Identity");

app.MapGet("/requires-auth", (ClaimsPrincipal user) => $"Hello, {user.Identity?.Name}!").RequireAuthorization()
    .WithName("BlazorApp_RequiresAuth")
    .WithTags("BlazorApp");

app.MapGet("/api/weatherforecast", async Task<Results<Ok<IEnumerable<WeatherForecast>>, BadRequest>> (DateOnly startDate, IWeatherForecastService weatherForecastService, CancellationToken cancellationToken) =>
    {
        var forecasts = await weatherForecastService.GetWeatherForecasts(startDate, cancellationToken);
        return TypedResults.Ok(forecasts);
    })
    .WithName("WeatherForecast_GetWeatherForecast")
    .WithTags("WeatherForecast")
    .WithOpenApi();;

app.Run();

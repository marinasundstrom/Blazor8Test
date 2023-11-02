using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks();
        /*
        .AddDbContextCheck<ApplicationDbContext>();
        */

        return services;
    }
}

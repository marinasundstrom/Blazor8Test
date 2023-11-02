using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions;

public static class FeatureManagementExtensions
{
    public static IServiceCollection AddFeatureManagement(this IServiceCollection services)
    {
        Microsoft.FeatureManagement.ServiceCollectionExtensions.AddFeatureManagement(services)
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<TimeWindowFilter>();

        return services;
    }
}

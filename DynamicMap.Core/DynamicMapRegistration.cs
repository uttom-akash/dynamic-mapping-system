using DynamicMappingLibrary.Context;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicMappingLibrary;

public static class DynamicMapRegistration
{
    public static IServiceCollection AddDynamicMap(this IServiceCollection services, MapContext context)
    {
        services.AddSingleton(context)
            .AddSingleton<IMapHandler, MapHandler>();

        return services;
    }
}
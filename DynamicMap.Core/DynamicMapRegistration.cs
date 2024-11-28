using DynamicMappingLibrary.Configurations;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicMappingLibrary;

public static class DynamicMapRegistration
{
    public static IServiceCollection AddDynamicMap(this IServiceCollection services,
        Type contextType)
    {
        services.AddSingleton(contextType)
            .AddSingleton(typeof(MapConfiguration), contextType)
            .AddSingleton<IMapHandler, MapHandler>();

        return services;
    }
}
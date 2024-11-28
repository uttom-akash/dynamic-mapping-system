using DynamicMappingLibrary.Common.Exceptions;
using DynamicMappingLibrary.Configurations;
using DynamicMappingLibrary.Contracts;
using Microsoft.Extensions.Logging;

namespace DynamicMappingLibrary.Handlers;

public class MapHandler : IMapHandler
{
    private readonly ILogger<MapHandler> _logger;
    private readonly MapConfiguration _mapConfiguration;

    public MapHandler(MapConfiguration mapConfiguration,
        ILogger<MapHandler> logger)
    {
        _mapConfiguration = mapConfiguration;
        _logger = logger;
    }

    public MapHandler(MapConfiguration mapConfiguration)
    {
        _mapConfiguration = mapConfiguration;
        _logger = CreateLogger();
    }

    public object Map(object source, string sourceType, string targetType)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(source);

            // Create map handler context and hand over the mapping task  
            var handlerContext =
                MapHandlerContext.Create(_mapConfiguration, _logger, _mapConfiguration.MaxRecursionDepth);

            var target = handlerContext.Map(source, sourceType, targetType);

            if (target is null)
                throw new NullMappingResultException($"Could not map from {sourceType} to {targetType}");

            return target;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Mapping exception during {sourceType} -> {targetType} for source object: {source}");
            throw;
        }
    }

    private ILogger<MapHandler> CreateLogger()
    {
        //Todo: Address the case when expected log destination is not console.
        using var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        return loggerFactory.CreateLogger<MapHandler>();
    }
}
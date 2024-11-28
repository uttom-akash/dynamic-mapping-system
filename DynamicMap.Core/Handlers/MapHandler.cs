using DynamicMappingLibrary.Configurations;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Exceptions;
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

    public object Map(object src, string sourceType, string targetType)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(src);

            var handlerContext =
                MapHandlerContext.Create(_mapConfiguration, _logger, _mapConfiguration.MaxRecursionDepth);

            var target = handlerContext.Map(src, sourceType, targetType);

            if (target is null)
                throw new NullMappingResultException($"Could not map from {sourceType} to {targetType}");

            return target;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Dynamic Map exception {sourceType} -> {targetType} for source object: {src}");
            throw;
        }
    }
}
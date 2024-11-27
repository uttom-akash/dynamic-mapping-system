using DynamicMappingLibrary.Context;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Exceptions;
using Microsoft.Extensions.Logging;

namespace DynamicMappingLibrary.Handlers;

public class MapHandler : IMapHandler
{
    private readonly ILogger<MapHandler> _logger;
    private readonly MapContext _mapContext;

    public MapHandler(MapContext mapContext,
        ILogger<MapHandler> logger)
    {
        _mapContext = mapContext;
        _logger = logger;
    }

    public object Map(object src, string sourceType, string targetType)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(src);

            var handlerContext = MapHandlerContext.Create(_mapContext, _logger, _mapContext.MaxRecursionDepth);

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
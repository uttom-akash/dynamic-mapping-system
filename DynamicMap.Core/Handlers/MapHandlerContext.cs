using DynamicMappingLibrary.Configurations;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Exceptions;
using Microsoft.Extensions.Logging;

namespace DynamicMappingLibrary.Handlers;

public class MapHandlerContext : IMapHandlerContext
{
    private readonly ILogger<MapHandler> _logger;

    private readonly MapConfiguration _mapConfiguration;

    public MapHandlerContext(MapConfiguration mapConfiguration,
        ILogger<MapHandler> logger,
        int recursionDepth)
    {
        _mapConfiguration = mapConfiguration;
        _logger = logger;
        RecursionDepth = recursionDepth;
    }

    private int RecursionDepth { get; }

    public object? Map(object? src, string sourceType, string targetType)
    {
        if (src is null)
        {
            _logger.LogDebug($"Lower level source object {sourceType} is null");
            return null;
        }

        if (HasExceddedRecursionDepth())
        {
            _logger.LogDebug($"Exceeded maximum recursion depth: {_mapConfiguration.MaxRecursionDepth}");

            return null;
        }

        var mapFunc = _mapConfiguration
            .GetMapFunc(sourceType, targetType);

        var childHandlerContext = Create(_mapConfiguration,
            _logger,
            RecursionDepth - 1);

        var target = mapFunc.Invoke(src, childHandlerContext);

        if (target is null)
            throw new NullMappingResultException($"Could not map from {sourceType} to {targetType}");

        return target;
    }

    private bool HasExceddedRecursionDepth()
    {
        return RecursionDepth <= 0;
    }

    public static IMapHandlerContext Create(MapConfiguration mapConfiguration, ILogger<MapHandler> logger,
        int recursionDepth)
    {
        return new MapHandlerContext(mapConfiguration, logger, recursionDepth);
    }
}
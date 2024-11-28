using DynamicMappingLibrary.Common.Exceptions;
using DynamicMappingLibrary.Contracts;

namespace DynamicMappingLibrary.Configurations;

/// <summary>
///     <c>MapConfiguration</c> is a configurations store that provides interface to add and retrieve map configurations
/// </summary>
public class MapConfiguration
{
    private readonly Dictionary<string, Func<object, IMapHandlerContext, object?>> _configurationStore = new();

    private int _maxRecursionDepth = 3;

    public MapConfiguration()
    {
    }

    public MapConfiguration(int maxRecursionDepth)
    {
        MaxRecursionDepth = maxRecursionDepth;
    }

    public int MaxRecursionDepth
    {
        get => _maxRecursionDepth;
        protected set
        {
            if (value < 1)
                throw new ArgumentException("MaxRecursionDepth must be at least 1.");

            if (value > 100)
                throw new ArgumentException("MaxRecursionDepth cannot exceed 100.");

            _maxRecursionDepth = value;
        }
    }

    /// <summary>
    ///     Adds a new map configuration between a source type and a target type.
    /// </summary>
    /// <param name="sourceType">The type of the source to be mapped.</param>
    /// <param name="targetType">The type of the target to map to.</param>
    /// <param name="mapStrategy"> A <see cref="IMapStrategy" /> to use during the mapping process.</param>
    /// <returns>A <see cref="ReverseMapState" /> to configure reverse mapping.</returns>
    public ReverseMapState AddMap(string sourceType, string targetType, IMapStrategy mapStrategy)
    {
        AddMap(sourceType, targetType, mapStrategy.Map);

        return new ReverseMapState(targetType, sourceType, mapStrategy.ReverseMap, this);
    }

    /// <summary>
    ///     Adds a new map configuration between a source type and a target type.
    /// </summary>
    /// <param name="sourceType">The type of the source to be mapped.</param>
    /// <param name="targetType">The type of the target to map to.</param>
    /// <param name="mapFunc">
    ///     A function that defines the mapping logic between the source and target types.
    ///     It takes source object as argument and returns the target object.
    /// </param>
    public void AddMap(string sourceType, string targetType, Func<object, IMapHandlerContext, object?> mapFunc)
    {
        var configKey = CreateKey(sourceType, targetType);

        if (_configurationStore.ContainsKey(configKey))
            throw new MappingRulesAlreadyExistsException(
                $"Mapping already configured for {sourceType} to {targetType}");

        _configurationStore.Add(configKey, mapFunc);
    }

    internal Func<object, IMapHandlerContext, object?> GetMapFunc(string sourceType, string targetType)
    {
        var configKey = CreateKey(sourceType, targetType);

        if (!_configurationStore.TryGetValue(configKey, out var mapFunc))
            throw new MappingRulesNotFoundException(
                $"There is no map configured for this types {sourceType} and {targetType}.");

        return mapFunc;
    }

    private static string CreateKey(string sourceType, string targetType)
    {
        return sourceType + "|" + targetType;
    }
}
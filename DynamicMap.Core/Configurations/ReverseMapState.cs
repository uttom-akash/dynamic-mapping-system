using DynamicMappingLibrary.Contracts;

namespace DynamicMappingLibrary.Configurations;

/// <summary>
///     <c>ReverseMapState</c> contains the required state and method to configure reverse mapping
/// </summary>
public class ReverseMapState
{
    private readonly MapConfiguration _mapConfiguration;
    private readonly Func<object, IMapHandlerContext, object?> _reverseMapperFunc;
    private readonly string _sourceType;
    private readonly string _targetType;

    public ReverseMapState(string sourceType,
        string targetType,
        Func<object, IMapHandlerContext, object?> reverseMapperFunc,
        MapConfiguration mapConfiguration)
    {
        _sourceType = sourceType;
        _targetType = targetType;
        _reverseMapperFunc = reverseMapperFunc;
        _mapConfiguration = mapConfiguration;
    }

    /// <summary>
    ///     Adds the reverse map configuration.
    /// </summary>
    public void AddReverseMap()
    {
        _mapConfiguration.AddMap(_sourceType, _targetType, _reverseMapperFunc);
    }
}
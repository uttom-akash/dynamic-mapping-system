using DynamicMappingLibrary.Contracts;

namespace DynamicMappingLibrary.Context;

/// <summary>
///     <c>ReverseMapState</c> contains the required state and method to configure reverse mapping
/// </summary>
public class ReverseMapState
{
    private readonly MapContext _mapContext;
    private readonly Func<object, IMapHandlerContext, object?> _reverseMapperFunc;
    private readonly string _sourceType;
    private readonly string _targetType;

    public ReverseMapState(string sourceType,
        string targetType,
        Func<object, IMapHandlerContext, object?> reverseMapperFunc,
        MapContext mapContext)
    {
        _sourceType = sourceType;
        _targetType = targetType;
        _reverseMapperFunc = reverseMapperFunc;
        _mapContext = mapContext;
    }

    /// <summary>
    ///     Adds the reverse map configuration.
    /// </summary>
    public void AddReverseMap()
    {
        _mapContext.AddMap(_sourceType, _targetType, _reverseMapperFunc);
    }
}
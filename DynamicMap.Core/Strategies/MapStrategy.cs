using DynamicMappingLibrary.Common.Helpers;
using DynamicMappingLibrary.Contracts;

namespace DynamicMappingLibrary.Strategies;

/// <summary>
///     Defines a strongly typed contract to provide mapping logics between source and target object, including reverse
///     mapping.
/// </summary>
public abstract class MapStrategy<TSource, TTarget> : IMapStrategy
{
    public object? Map(object source, IMapHandlerContext handlerContext)
    {
        var typeCastedSource = TypeCastUtil.CastTypeBeforeMap<TSource>(source);

        var target = Map(typeCastedSource, handlerContext);

        var typedCastedTarget = TypeCastUtil.CastTypeAfterMap<TTarget>(target);

        return typedCastedTarget;
    }

    public object? ReverseMap(object target, IMapHandlerContext handlerContext)
    {
        var typeCastedTarget = TypeCastUtil.CastTypeBeforeMap<TTarget>(target);

        var source = ReverseMap(typeCastedTarget, handlerContext);

        var typedCastedSource = TypeCastUtil.CastTypeAfterMap<TSource>(source);

        return typedCastedSource;
    }

    /// <summary>
    ///     Maps an object of type <typeparamref name="TSource" /> to an object of type <typeparamref name="TTarget" />.
    /// </summary>
    /// <param name="source">The source object to be mapped.</param>
    /// <returns>The mapped object of type <typeparamref name="TTarget" />.</returns>
    /// <param name="handlerContext"> A context to map nested object further</param>
    public abstract TTarget Map(TSource source, IMapHandlerContext handlerContext);

    /// <summary>
    ///     Maps an object of type <typeparamref name="TTarget" /> back to an object of type <typeparamref name="TSource" />.
    /// </summary>
    /// <param name="target">The target object to be reverse-mapped.</param>
    /// <returns>The mapped object of type <typeparamref name="TSource" />.</returns>
    /// <param name="handlerContext"> A context to map nested object further</param>
    public abstract TSource ReverseMap(TTarget target, IMapHandlerContext handlerContext);
}
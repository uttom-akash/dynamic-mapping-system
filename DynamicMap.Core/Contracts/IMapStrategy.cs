using DynamicMappingLibrary.Exceptions;

namespace DynamicMappingLibrary.Contracts;

/// <summary>
///     Defines a contract to provide mapping logics between source and target object, including reverse mapping.
/// </summary>
public interface IMapStrategy
{
    /// <summary>
    ///     Maps the provided source object to a target object based on the implemented strategy.
    ///     Throws an <see cref="ArgumentNullException" /> if the source is null.
    ///     Throws an <see cref="InvalidSourceTypeException" /> if the source is not of the expected type.
    ///     Throws a <see cref="NullMappingResultException" /> if the mapping result is null.
    /// </summary>
    /// <param name="source">The source object to be mapped.</param>
    /// <returns>The mapped target object.</returns>
    object? Map(object source, IMapHandlerContext handlerContext);

    /// <summary>
    ///     Maps the provided target object back to a source object based on the implemented strategy.
    ///     Throws an <see cref="ArgumentNullException" /> if the source is null.
    ///     Throws an <see cref="InvalidTargetTypeException" /> if the target is not of the expected type.
    ///     Throws a <see cref="NullMappingResultException" /> if the mapping result is null.
    /// </summary>
    /// <param name="target">The target object to be reverse-mapped.</param>
    /// <returns>The mapped source object.</returns>
    object? ReverseMap(object target, IMapHandlerContext handlerContext);
}
namespace DynamicMappingLibrary.Contracts;

/// <summary>
///     Defines a contract for handling object mappings between different types.
/// </summary>
public interface IMapHandler
{
    /// <summary>
    ///     Maps an object from a specified source type to a target type.
    ///     Throws an <see cref="ArgumentNullException" /> if the source is null.
    ///     Throws an <see cref="InvalidSourceTypeException" /> if the source is not of the expected type.
    ///     Throws an <see cref="InvalidTargetTypeException" /> if the target is not of the expected type.
    ///     Throws a <see cref="MappingResultNullException" /> if the mapping result is null.
    /// </summary>
    /// <param name="source">The source object to be mapped.</param>
    /// <param name="sourceType">The name of the source type.</param>
    /// <param name="targetType">The name of the target type.</param>
    /// <returns>The mapped object of the target type.</returns>
    object Map(object source, string sourceType, string targetType);
}
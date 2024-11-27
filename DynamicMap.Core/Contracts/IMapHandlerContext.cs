namespace DynamicMappingLibrary.Contracts;

public interface IMapHandlerContext
{
    object? Map(object? src, string sourceType, string targetType);
}
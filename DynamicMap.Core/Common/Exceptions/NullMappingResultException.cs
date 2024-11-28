namespace DynamicMappingLibrary.Common.Exceptions;

public class NullMappingResultException : MappingException
{
    public NullMappingResultException(string message)
        : base(message)
    {
    }

    public NullMappingResultException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
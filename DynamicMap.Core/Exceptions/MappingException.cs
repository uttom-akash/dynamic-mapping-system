namespace DynamicMappingLibrary.Exceptions;

public class MappingException : Exception
{
    public MappingException(string message)
        : base(message)
    {
    }

    public MappingException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
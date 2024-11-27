namespace DynamicMappingLibrary.Exceptions;

public class MappingRulesAlreadyExistsException : MappingException
{
    public MappingRulesAlreadyExistsException(string message)
        : base(message)
    {
    }

    public MappingRulesAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
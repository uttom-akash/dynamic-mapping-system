namespace DynamicMappingLibrary.Common.Exceptions;

public class MappingRulesNotFoundException : MappingException
{
    public MappingRulesNotFoundException(string message)
        : base(message)
    {
    }

    public MappingRulesNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
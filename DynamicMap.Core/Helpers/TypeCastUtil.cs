namespace DynamicMappingLibrary.Helpers;

public static class TypeCastUtil
{
    public static TExpected CastTypeBeforeMap<TExpected>(object obj)
    {
        ValidateType<TExpected>(obj,
            new InvalidCastException($"The provided object could not be casted to a {typeof(TExpected).FullName}"));

        return (TExpected)obj;
    }

    public static TExpected? CastTypeAfterMap<TExpected>(object? obj)
    {
        ValidateType<TExpected>(obj,
            new InvalidCastException($"The mapped object could not be casted to a {typeof(TExpected).FullName}"));

        return (TExpected?)obj;
    }

    private static void ValidateType<TExpected>(object? obj, InvalidCastException exception)
    {
        if (obj is not TExpected) throw exception;
    }
}
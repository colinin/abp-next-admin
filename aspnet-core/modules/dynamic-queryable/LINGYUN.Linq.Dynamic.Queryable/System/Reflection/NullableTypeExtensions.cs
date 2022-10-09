namespace System.Reflection;

public static class NullableTypeExtensions
{
    public static bool IsNullableType(this Type theType)
    {
        return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
    }
}

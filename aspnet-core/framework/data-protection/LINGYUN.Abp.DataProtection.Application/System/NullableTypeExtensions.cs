namespace System;

internal static class NullableTypeExtensions
{
    public static bool IsNullableType(this Type theType) =>
        theType.IsGenericType(typeof(Nullable<>));

    public static bool IsGenericType(this Type type, Type genericType) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
}

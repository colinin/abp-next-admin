using System;

namespace System.Reflection;

internal static class NullableTypeExtensions
{
    public static bool IsNullableType(this Type theType)
    {
        return theType.IsGenericType(typeof(Nullable<>));
    }

    public static bool IsGenericType(this Type type, Type genericType)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}

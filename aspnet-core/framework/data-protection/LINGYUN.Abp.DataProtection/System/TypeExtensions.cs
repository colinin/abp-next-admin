using System.Collections;

namespace System;
internal static class TypeExtensions
{
    public static bool IsArrayOrListType(this Type type)
    {
        if (type.IsArray)
        {
            return true;
        }

        if (type.IsGenericType)
        {
            if (typeof(IList).IsAssignableFrom(type))
            {
                return true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return true;
            }
        }

        return false;
    }
}

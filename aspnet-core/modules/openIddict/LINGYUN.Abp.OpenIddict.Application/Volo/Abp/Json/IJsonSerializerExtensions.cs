using System;
using System.Collections.Generic;

namespace Volo.Abp.Json;
internal static class IJsonSerializerExtensions
{
    public static List<TResult> DeserializeToList<TResult>(this IJsonSerializer serializer, string source)
    {
        if (source.IsNullOrWhiteSpace())
        {
            return new List<TResult>();
        }

        return serializer.Deserialize<List<TResult>>(source);
    }

    public static Dictionary<TKey, TValue> DeserializeToDictionary<TKey, TValue>(this IJsonSerializer serializer, string source)
    {
        if (source.IsNullOrWhiteSpace())
        {
            return new Dictionary<TKey, TValue>();
        }

        return serializer.Deserialize<Dictionary<TKey, TValue>>(source);
    }
}

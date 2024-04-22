using System.Collections.Generic;

namespace System.Text.Json
{
    internal static class JsonElementExtensions
    {
        public static IEnumerable<string> GetRootStrings(this JsonDocument json, string key)
        {
            return json.RootElement.GetStrings(key);
        }

        public static IEnumerable<string> GetStrings(this JsonElement json, string key)
        {
            var result = new List<string>();

            if (json.TryGetProperty(key, out JsonElement property) && property.ValueKind == JsonValueKind.Array)
            {
                foreach (var jsonProp in property.EnumerateArray())
                {
                    result.Add(jsonProp.GetString());
                }
            }

            return result;
        }

        public static string GetRootString(this JsonDocument json, string key, string defaultValue = "")
        {
            if (json.RootElement.TryGetProperty(key, out JsonElement property))
            {
                return property.GetString();
            }
            return defaultValue;
        }

        public static string GetString(this JsonElement json, string key, string defaultValue = "")
        {
            if (json.TryGetProperty(key, out JsonElement property))
            {
                return property.GetString();
            }
            return defaultValue;
        }

        public static int GetRootInt32(this JsonDocument json, string key, int defaultValue = 0)
        {
            if (json.RootElement.TryGetProperty(key, out JsonElement property) && property.TryGetInt32(out int value))
            {
                return value;
            }
            return defaultValue;
        }

        public static int GetInt32(this JsonElement json, string key, int defaultValue = 0)
        {
            if (json.TryGetProperty(key, out JsonElement property) && property.TryGetInt32(out int value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}

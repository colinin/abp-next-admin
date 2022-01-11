using System;

namespace Quartz;

public static class IJobExecutionContextExtensions
{
    public static TValue GetData<TValue>(this IJobExecutionContext context, string key)
    {
        var value = context.MergedJobDataMap.GetString(key);

        return (TValue)Convert.ChangeType(value, typeof(TValue));
    }

    public static string GetString(this IJobExecutionContext context, string key)
    {
        var value = context.MergedJobDataMap.Get(key);

        return value != null ? value.ToString() : "";
    }

    public static int GetInt(this IJobExecutionContext context, string key)
    {
        var value = context.MergedJobDataMap.GetInt(key);

        return value;
    }
}

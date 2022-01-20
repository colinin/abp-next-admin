using LINGYUN.Abp.BackgroundTasks;
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

    public static bool TryGetMultiTenantId(this IJobExecutionContext context, out Guid? tenantId)
    {
        return context.TryGetMultiTenantId(nameof(JobInfo.TenantId), out tenantId);
    }

    public static bool TryGetMultiTenantId(this IJobExecutionContext context, string multiTenancyKey, out Guid? tenantId)
    {
        tenantId = null;
        var tenantUUIdString = context.GetString(multiTenancyKey);
        if (Guid.TryParse(tenantUUIdString, out var tenantUUId))
        {
            tenantId = tenantUUId;
            return true;
        }
        return false;
    }
}

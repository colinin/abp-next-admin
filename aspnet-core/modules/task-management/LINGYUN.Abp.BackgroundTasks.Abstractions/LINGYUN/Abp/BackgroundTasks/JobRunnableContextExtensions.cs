using Microsoft.Extensions.DependencyInjection;
using System;

namespace LINGYUN.Abp.BackgroundTasks;

public static class JobRunnableContextExtensions
{
    public static T GetService<T>(this JobRunnableContext context)
    {
        return context.ServiceProvider.GetService<T>();
    }

    public static object GetService(this JobRunnableContext context, Type serviceType)
    {
        return context.ServiceProvider.GetService(serviceType);
    }

    public static T GetRequiredService<T>(this JobRunnableContext context)
    {
        return context.ServiceProvider.GetRequiredService<T>();
    }

    public static object GetRequiredService(this JobRunnableContext context, Type serviceType)
    {
        return context.ServiceProvider.GetRequiredService(serviceType);
    }

    public static string GetString(this JobRunnableContext context, string key)
    {
        return context.GetJobData(key).ToString();
    }

    public static bool TryGetString(this JobRunnableContext context, string key, out string value)
    {
        if (context.TryGetJobData(key, out var data) && data != null)
        {
            value = data.ToString();
            return true;
        }
        value = default;
        return false;
    }

    public static T GetJobData<T>(this JobRunnableContext context, string key) where T : struct
    {
        var value = context.GetJobData(key);

        return value.To<T>();
    }

    public static bool TryGetJobData<T>(this JobRunnableContext context, string key, out T value) where T : struct
    {
        if (context.TryGetJobData(key, out var data) && data != null)
        {
            try
            {
                value = data.To<T>();
                return true;
            }
            catch
            {
            }
        }
        value = default;
        return false;
    }

    public static object GetJobData(this JobRunnableContext context, string key)
    {
        if (context.TryGetJobData(key, out var value) && value != null)
        {
            return value;
        }
        throw new ArgumentException($"Job required data [{key}] not specified.");
    }

    public static bool TryGetJobData(this JobRunnableContext context, string key, out object value)
    {
        if (context.JobData.TryGetValue(key, out value))
        {
            return true;
        }
        return false;
    }
}

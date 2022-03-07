using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobRunnableContext
{
    public Type JobType { get; }
    public IServiceProvider ServiceProvider { get; }
    public IReadOnlyDictionary<string, object> JobData { get; }
    public object Result { get; private set; }
    private Func<object, object> GetCacheData { get; set; }
    private Action<object, object> SetCacheData { get; set; }
    public JobRunnableContext(
        Type jobType,
        IServiceProvider serviceProvider,
        IReadOnlyDictionary<string, object> jobData,
        Func<object, object> getCache = null,
        Action<object, object> setCache = null)
    {
        JobType = jobType;
        ServiceProvider = serviceProvider;
        JobData = jobData;

        GetCacheData = getCache;
        SetCacheData = setCache;
    }

    public void SetResult(object result)
    {
        Result = result;
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetCache(object key, object value)
    {
        SetCacheData?.Invoke(key, value);
    }

#nullable enable
    /// <summary>
    /// 获取缓存数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object? GetCache(object key)
    {
        if (GetCacheData != null)
        {
            return GetCacheData(key);
        }
        return null;
    }
#nullable disable
}

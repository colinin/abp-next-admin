using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.BackgroundTasks;
public static class JobDispatcherSelectorListExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="selectors"></param>
    /// <param name="setup"></param>
    /// <remarks>
    /// Tips: 仅作用于适用于<see cref="IBackgroundJobManager" /> 接口的作业预配置
    /// </remarks>
    public static void AddJob<TJob>([NotNull] this IJobDispatcherSelectorList selectors, [CanBeNull] Action<JobTypeSelector> setup = null)
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Job:" + typeof(TJob).FullName;
        if (selectors.Any(s => s.Name == selectorName))
        {
            return;
        }

        var selector = new JobTypeSelector(selectorName, t => typeof(TJob).IsAssignableFrom(t));

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static void RemoveJob<TJob>([NotNull] this IJobDispatcherSelectorList selectors)
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Job:" + typeof(TJob).FullName;
        selectors.RemoveAll(s => s.Name == selectorName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TWorker"></typeparam>
    /// <param name="selectors"></param>
    /// <param name="setup"></param>
    /// <remarks>
    /// Tips: 仅作用于适用于<see cref="IBackgroundWorker" /> 接口的作业预配置
    /// </remarks>
    public static void AddWorker<TWorker>([NotNull] this IJobDispatcherSelectorList selectors, [CanBeNull] Action<JobTypeSelector> setup = null)
        where TWorker : IBackgroundWorker
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Worker:" + typeof(TWorker).FullName;
        if (selectors.Any(s => s.Name == selectorName))
        {
            return;
        }

        var selector = new JobTypeSelector(selectorName, t => typeof(TWorker).IsAssignableFrom(t));

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static void RemoveWorker<TWorker>([NotNull] this IJobDispatcherSelectorList selectors)
        where TWorker : IBackgroundWorker
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Worker:" + typeof(TWorker).FullName;
        selectors.RemoveAll(s => s.Name == selectorName);
    }
}

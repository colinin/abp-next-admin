using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.BackgroundTasks;
public static class JobDispatcherSelectorListExtensions
{
    public const string AllJobssSelectorName = "All";

    public static void AddNamespace(
        [NotNull] this IJobDispatcherSelectorList selectors,
        [NotNull] string namespaceName,
        [CanBeNull] Action<JobTypeSelector> setup = null)
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Namespace:" + namespaceName;
        if (selectors.Any(s => s.Name == selectorName))
        {
            return;
        }

        var selector = new JobTypeSelector(selectorName, t => t.FullName?.StartsWith(namespaceName) ?? false);

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static void Add<TJob>([NotNull] this IJobDispatcherSelectorList selectors, [CanBeNull] Action<JobTypeSelector> setup = null)
        where TJob : IJobRunnable
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

    public static void Remove<TJob>([NotNull] this IJobDispatcherSelectorList selectors)
        where TJob : IJobRunnable
    {
        Check.NotNull(selectors, nameof(selectors));

        var selectorName = "Job:" + typeof(TJob).FullName;
        selectors.RemoveAll(s => s.Name == selectorName);
    }

    public static void AddAll([NotNull] this IJobDispatcherSelectorList selectors, [CanBeNull] Action<JobTypeSelector> setup = null)
    {
        Check.NotNull(selectors, nameof(selectors));

        if (selectors.Any(s => s.Name == AllJobssSelectorName))
        {
            return;
        }

        var selector = new JobTypeSelector(AllJobssSelectorName, t => typeof(IJobRunnable).IsAssignableFrom(t));

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static void Add(
        [NotNull] this IJobDispatcherSelectorList selectors,
        string selectorName,
        Func<Type, bool> predicate,
        [CanBeNull] Action<JobTypeSelector> setup = null)
    {
        Check.NotNull(selectors, nameof(selectors));

        if (selectors.Any(s => s.Name == selectorName))
        {
            throw new AbpException($"There is already a selector added before with the same name: {selectorName}");
        }

        var selector = new JobTypeSelector(selectorName, predicate);

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static void Add(
        [NotNull] this IJobDispatcherSelectorList selectors,
        Func<Type, bool> predicate,
        [CanBeNull] Action<JobTypeSelector> setup = null)
    {
        var selector = new JobTypeSelector(Guid.NewGuid().ToString("N"), predicate);

        setup?.Invoke(selector);

        selectors.Add(selector);
    }

    public static bool RemoveByName(
        [NotNull] this IJobDispatcherSelectorList selectors,
        [NotNull] string name)
    {
        Check.NotNull(selectors, nameof(selectors));
        Check.NotNull(name, nameof(name));

        return selectors.RemoveAll(s => s.Name == name).Count > 0;
    }

    public static bool IsMatch([NotNull] this IJobDispatcherSelectorList selectors, Type jobType)
    {
        Check.NotNull(selectors, nameof(selectors));
        return selectors.Any(s => s.Predicate(jobType));
    }
}

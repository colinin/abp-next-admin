using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

internal class JobEventProvider : IJobEventProvider, ISingletonDependency
{
    private readonly Lazy<List<IJobEvent>> _lazyEvents;
    private List<IJobEvent> _events => _lazyEvents.Value;

    private readonly IServiceProvider _serviceProvider;
    private readonly AbpBackgroundTasksOptions _options;
    public JobEventProvider(
        IOptions<AbpBackgroundTasksOptions> options,
        IServiceProvider serviceProvider)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;

        _lazyEvents = new Lazy<List<IJobEvent>>(CreateJobEvents);
    }
    public IReadOnlyCollection<IJobEvent> GetAll()
    {
        return _events.ToImmutableList();
    }

    private List<IJobEvent> CreateJobEvents()
    {
        var jobEvents = _options
                .JobMonitors
                .Select(p => _serviceProvider.GetRequiredService(p) as IJobEvent)
                .ToList();

        return jobEvents;
    }
}

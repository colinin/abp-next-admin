using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobDefinitionManager : IJobDefinitionManager, ISingletonDependency
{
    protected Lazy<IDictionary<string, JobDefinition>> JobDefinitions { get; }

    protected AbpBackgroundTasksOptions Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public JobDefinitionManager(
        IOptions<AbpBackgroundTasksOptions> options,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;

        JobDefinitions = new Lazy<IDictionary<string, JobDefinition>>(CreateJobDefinitions, true);
    }

    public virtual JobDefinition Get(string name)
    {
        Check.NotNull(name, nameof(name));

        var action = GetOrNull(name);

        if (action == null)
        {
            throw new AbpException("Undefined job: " + name);
        }

        return action;
    }

    public virtual IReadOnlyList<JobDefinition> GetAll()
    {
        return JobDefinitions.Value.Values.ToImmutableList();
    }

    public virtual JobDefinition GetOrNull(string name)
    {
        return JobDefinitions.Value.GetOrDefault(name);
    }

    protected virtual IDictionary<string, JobDefinition> CreateJobDefinitions()
    {
        var jobs = new Dictionary<string, JobDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IJobDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider.Define(new JobDefinitionContext(jobs));
            }
        }

        return jobs;
    }
}

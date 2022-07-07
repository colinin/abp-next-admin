using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public class JobActionDefinitionManager : IJobActionDefinitionManager, ISingletonDependency
{
    protected Lazy<IDictionary<string, JobActionDefinition>> ActionDefinitions { get; }

    protected AbpBackgroundTasksActivitiesOptions Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public JobActionDefinitionManager(
        IOptions<AbpBackgroundTasksActivitiesOptions> options,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;

        ActionDefinitions = new Lazy<IDictionary<string, JobActionDefinition>>(CreateSettingDefinitions, true);
    }

    public virtual JobActionDefinition Get(string name)
    {
        Check.NotNull(name, nameof(name));

        var action = GetOrNull(name);

        if (action == null)
        {
            throw new AbpException("Undefined action: " + name);
        }

        return action;
    }

    public virtual IReadOnlyList<JobActionDefinition> GetAll()
    {
        return ActionDefinitions.Value.Values.ToImmutableList();
    }

    public virtual JobActionDefinition GetOrNull(string name)
    {
        return ActionDefinitions.Value.GetOrDefault(name);
    }

    protected virtual IDictionary<string, JobActionDefinition> CreateSettingDefinitions()
    {
        var actions = new Dictionary<string, JobActionDefinition>();

        using (var scope = ServiceProvider.CreateScope())
        {
            var providers = Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IJobActionDefinitionProvider)
                .ToList();

            foreach (var provider in providers)
            {
                provider.Define(new JobActionDefinitionContext(actions));
            }
        }

        return actions;
    }
}

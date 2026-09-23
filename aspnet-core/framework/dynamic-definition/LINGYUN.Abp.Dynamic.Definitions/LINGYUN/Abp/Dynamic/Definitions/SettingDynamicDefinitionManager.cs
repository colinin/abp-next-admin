using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Dynamic.Definitions;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ISettingDefinitionManager),
    typeof(SettingDynamicDefinitionManager))]
public class SettingDynamicDefinitionManager : 
    DynamicDefinitionManager<SettingDefinition>, 
    ISettingDefinitionManager, 
    ISingletonDependency
{
    protected IStaticSettingDefinitionStore StaticStore { get; }
    protected IDynamicSettingDefinitionStore DynamicStore { get; }
    public SettingDynamicDefinitionManager(
        IStaticSettingDefinitionStore staticStore,
        IDynamicSettingDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options) : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        var staticDefinitions = await StaticStore.GetAllAsync();
        var dynamicDefinitions = await DynamicStore.GetAllAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<SettingDefinition> GetAsync(string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined setting: " + name);
    }

    public async virtual Task<SettingDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(SettingDefinition definition)
    {
        return definition.Name;
    }

    protected override Task<SettingDefinition> MergeDefinitionAsync(SettingDefinition targetDefinition, SettingDefinition sourceDefinition)
    {
        targetDefinition.DisplayName = sourceDefinition.DisplayName;
        targetDefinition.Description = sourceDefinition.Description ?? targetDefinition.Description;
        targetDefinition.DefaultValue = sourceDefinition.DefaultValue ?? targetDefinition.DefaultValue;
        targetDefinition.IsVisibleToClients = targetDefinition.IsVisibleToClients || sourceDefinition.IsVisibleToClients;
        targetDefinition.IsInherited = targetDefinition.IsInherited || sourceDefinition.IsInherited;
        targetDefinition.IsEncrypted = targetDefinition.IsEncrypted || sourceDefinition.IsEncrypted;

        foreach (var property in sourceDefinition.Properties)
        {
            targetDefinition.Properties[property.Key] = property.Value;
        }

        foreach (var provider in sourceDefinition.Providers)
        {
            if (!targetDefinition.Providers.Contains(provider))
            {
                targetDefinition.Providers.Add(provider);
            }
        }

        return Task.FromResult(targetDefinition);
    }
}

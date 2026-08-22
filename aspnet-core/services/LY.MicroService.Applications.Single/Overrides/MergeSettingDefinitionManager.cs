using System.Collections.Immutable;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

#nullable enable

[Dependency(ReplaceServices = true)]
public class MergeSettingDefinitionManager : SettingDefinitionManager
{
    public MergeSettingDefinitionManager(
        IStaticSettingDefinitionStore staticStore,
        IDynamicSettingDefinitionStore dynamicStore)
        : base(staticStore, dynamicStore)
    {
    }

    public async override Task<SettingDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        if (staticDefinition != null && dynamicDefinition != null)
        {
            MergeSetting(staticDefinition, dynamicDefinition);
        }

        return staticDefinition ?? dynamicDefinition;
    }

    public async override Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        var staticSettings = await StaticStore.GetAllAsync();
        var dynamicSettings = await DynamicStore.GetAllAsync();

        var mergedSettings = new Dictionary<string, SettingDefinition>();

        foreach (var staticSetting in staticSettings)
        {
            mergedSettings[staticSetting.Name] = staticSetting;
        }

        foreach (var dynamicSetting in dynamicSettings)
        {
            if (mergedSettings.TryGetValue(dynamicSetting.Name, out var existingSetting))
            {
                MergeSetting(existingSetting, dynamicSetting);
            }
            else
            {
                mergedSettings[dynamicSetting.Name] = dynamicSetting;
            }
        }

        return mergedSettings.Values.ToImmutableList();
    }

    private static void MergeSetting(SettingDefinition target, SettingDefinition source)
    {
        target.DisplayName = source.DisplayName;
        target.Description = source.Description ?? target.Description;
        target.DefaultValue = source.DefaultValue ?? target.DefaultValue;
        target.IsVisibleToClients = target.IsVisibleToClients || source.IsVisibleToClients;
        target.IsInherited = target.IsInherited || source.IsInherited;
        target.IsEncrypted = target.IsEncrypted || source.IsEncrypted;

        foreach (var property in source.Properties)
        {
            target.Properties[property.Key] = property.Value;
        }

        foreach (var provider in source.Providers)
        {
            if (!target.Providers.Contains(provider))
            {
                target.Providers.Add(provider);
            }
        }
    }
}
#nullable disable


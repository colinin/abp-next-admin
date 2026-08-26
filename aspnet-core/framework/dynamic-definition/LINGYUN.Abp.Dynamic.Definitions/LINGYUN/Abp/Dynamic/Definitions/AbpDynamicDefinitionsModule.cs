using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Dynamic.Definitions;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpDynamicDefinitionsCoreModule))]
public class AbpDynamicDefinitionsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicDefinitionsOptions>(options =>
        {
            options.MapStrategy<FeatureDefinition>(DynamicDefinitionStrategy.Merge);
            options.MapStrategy<FeatureGroupDefinition>(DynamicDefinitionStrategy.Merge);

            options.MapStrategy<PermissionDefinition>(DynamicDefinitionStrategy.Merge);
            options.MapStrategy<PermissionGroupDefinition>(DynamicDefinitionStrategy.Merge);

            options.MapStrategy<SettingDefinition>(DynamicDefinitionStrategy.Merge);

            options.MapStrategy<TemplateDefinition>(DynamicDefinitionStrategy.Merge);
        });
    }
}

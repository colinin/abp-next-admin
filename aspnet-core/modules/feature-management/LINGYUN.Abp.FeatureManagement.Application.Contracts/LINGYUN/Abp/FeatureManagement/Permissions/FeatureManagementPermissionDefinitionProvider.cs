using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.FeatureManagement.Permissions;

public class FeatureManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var featureGroup = context.GetGroup(FeatureManagementPermissionNames.GroupName);

        var groupDefinition = featureGroup.AddPermission(
            FeatureManagementPermissionNames.GroupDefinition.Default,
            L("Permission:GroupDefinitions"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            FeatureManagementPermissionNames.GroupDefinition.Create, 
            L("Permission:Create"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            FeatureManagementPermissionNames.GroupDefinition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            FeatureManagementPermissionNames.GroupDefinition.Delete, 
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var featureDefinition = featureGroup.AddPermission(
            FeatureManagementPermissionNames.Definition.Default, 
            L("Permission:FeatureDefinitions"),
            MultiTenancySides.Host);
        featureDefinition.AddChild(
            FeatureManagementPermissionNames.Definition.Create, 
            L("Permission:Create"),
            MultiTenancySides.Host);
        featureDefinition.AddChild(
            FeatureManagementPermissionNames.Definition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        featureDefinition.AddChild(
            FeatureManagementPermissionNames.Definition.Delete, 
            L("Permission:Delete"),
            MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpFeatureManagementResource>(name);
    }
}

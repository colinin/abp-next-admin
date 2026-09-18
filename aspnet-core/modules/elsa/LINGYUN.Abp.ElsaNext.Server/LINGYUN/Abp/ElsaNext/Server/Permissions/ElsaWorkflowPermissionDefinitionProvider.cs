using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.Server.Permissions;

public class ElsaWorkflowPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var elsaPermission = context.GetGroupOrNull(ElsaPermissionNames.GroupName);
        if (elsaPermission == null)
        {
            return;
        }

        var definitionsPermission = elsaPermission.AddPermission(
            ElsaWorkflowPermissionNames.Definitions.Default,
            L("Permission:Definitions"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Execute,
            L("Permission:Execute"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Publish,
            L("Permission:Publish"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Retract,
            L("Permission:Retract"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Refresh,
            L("Permission:Refresh"),
            MultiTenancySides.Host);
        definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Reload,
            L("Permission:Reload"),
            MultiTenancySides.Host);
        var definitionsVesionPermission = definitionsPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Versions.Default,
            L("Permission:Versions"),
            MultiTenancySides.Host);
        definitionsVesionPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Versions.Revert,
            L("Permission:Revert"),
            MultiTenancySides.Host);
        definitionsVesionPermission.AddChild(
            ElsaWorkflowPermissionNames.Definitions.Versions.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var instancesPermission = elsaPermission.AddPermission(
           ElsaWorkflowPermissionNames.Instances.Default,
           L("Permission:Instances"),
           MultiTenancySides.Host);
        instancesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        instancesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
        instancesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);
        instancesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Cancel,
            L("Permission:Cancel"),
            MultiTenancySides.Host);
        var instancesVariablesPermission = instancesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Variables.Default,
            L("Permission:Variables"),
            MultiTenancySides.Host);
        instancesVariablesPermission.AddChild(
            ElsaWorkflowPermissionNames.Instances.Variables.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ElsaNextResource>(name);
    }
}

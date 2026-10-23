using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;

public class ElsaNextApprovalFlowPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var approvalFlow = context.AddGroup(
            ElsaNextApprovalFlowPermissions.GroupName,
            L10n("Permission:ApprovalFlow"));

        var templates = approvalFlow.AddPermission(
            ElsaNextApprovalFlowPermissions.Templates.Default,
            L10n("Permission:Templates"));
        templates.AddChild(ElsaNextApprovalFlowPermissions.Templates.Create, L10n("Permission:Templates.Create"));
        templates.AddChild(ElsaNextApprovalFlowPermissions.Templates.Update, L10n("Permission:Templates.Update"));
        templates.AddChild(ElsaNextApprovalFlowPermissions.Templates.Delete, L10n("Permission:Templates.Delete"));
        templates.AddChild(ElsaNextApprovalFlowPermissions.Templates.Publish, L10n("Permission:Templates.Publish"));

        var instances = approvalFlow.AddPermission(
            ElsaNextApprovalFlowPermissions.Instances.Default,
            L10n("Permission:Instances"));
        instances.AddChild(ElsaNextApprovalFlowPermissions.Instances.Create, L10n("Permission:Instances.Create"));
        instances.AddChild(ElsaNextApprovalFlowPermissions.Instances.Decide, L10n("Permission:Instances.Decide"));
        instances.AddChild(ElsaNextApprovalFlowPermissions.Instances.Withdraw, L10n("Permission:Instances.Withdraw"));
        instances.AddChild(ElsaNextApprovalFlowPermissions.Instances.Manage, L10n("Permission:Instances.Manage"));

        var formDefinitions = context.AddGroup(
            ElsaNextApprovalFlowPermissions.FormDefinitions.GroupName,
            L10n("Permission:FormDefinitions"));

        var definitions = formDefinitions.AddPermission(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Default,
            L10n("Permission:FormDefinitions.Definitions"));
        definitions.AddChild(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Create,
            L10n("Permission:FormDefinitions.Definitions.Create"));
        definitions.AddChild(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update,
            L10n("Permission:FormDefinitions.Definitions.Update"));
        definitions.AddChild(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Delete,
            L10n("Permission:FormDefinitions.Definitions.Delete"));
        definitions.AddChild(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Publish,
            L10n("Permission:FormDefinitions.Definitions.Publish"));

        var data = formDefinitions.AddPermission(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Default,
            L10n("Permission:FormDefinitions.Data"));
        data.AddChild(
            ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Edit,
            L10n("Permission:FormDefinitions.Data.Edit"));
    }

    private static LocalizableString L10n(string name)
    {
        return LocalizableString.Create<Localization.ElsaNextApprovalFlowResource>(name);
    }
}

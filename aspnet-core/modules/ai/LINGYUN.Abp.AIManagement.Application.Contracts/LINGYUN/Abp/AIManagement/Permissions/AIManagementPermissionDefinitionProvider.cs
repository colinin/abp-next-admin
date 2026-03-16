using LINGYUN.Abp.AIManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Permissions;

public class AIManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(AIManagementPermissionNames.GroupName, L("Permission:AIManagement"));

        var groupDefinition = group.AddPermission(
            AIManagementPermissionNames.WorkspaceDefinition.Default,
            L("Permission:WorkspaceDefinition"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            AIManagementPermissionNames.WorkspaceDefinition.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            AIManagementPermissionNames.WorkspaceDefinition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            AIManagementPermissionNames.WorkspaceDefinition.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var conversation = group.AddPermission(
            AIManagementPermissionNames.Conversation.Default,
            L("Permission:Conversation"));
        conversation.AddChild(
            AIManagementPermissionNames.Conversation.Create,
            L("Permission:Create"));
        conversation.AddChild(
            AIManagementPermissionNames.Conversation.Update,
            L("Permission:Update"));
        conversation.AddChild(
            AIManagementPermissionNames.Conversation.Delete,
            L("Permission:Delete"));

        var chat = group.AddPermission(
            AIManagementPermissionNames.Chat.Default,
            L("Permission:Chats"));
        chat.AddChild(
            AIManagementPermissionNames.Chat.SendMessage,
            L("Permission:SendMessage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AIManagementResource>(name);
    }
}

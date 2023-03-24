using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement.Authorization;

public class WebhooksManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            WebhooksManagementPermissions.GroupName, 
            L("Permission:WebhooksManagement"));

        var webhookGroupDefinition = group.AddPermission(
            WebhooksManagementPermissions.WebhookGroupDefinition.Default,
            L("Permission:WebhookGroupDefinitions"),
            MultiTenancySides.Host);
        webhookGroupDefinition.AddChild(
            WebhooksManagementPermissions.WebhookGroupDefinition.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        webhookGroupDefinition.AddChild(
            WebhooksManagementPermissions.WebhookGroupDefinition.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
        webhookGroupDefinition.AddChild(
            WebhooksManagementPermissions.WebhookGroupDefinition.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var webhookDefinition = group.AddPermission(
            WebhooksManagementPermissions.WebhookDefinition.Default,
            L("Permission:WebhookDefinitions"),
            MultiTenancySides.Host);
        webhookDefinition.AddChild(
            WebhooksManagementPermissions.WebhookDefinition.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        webhookDefinition.AddChild(
            WebhooksManagementPermissions.WebhookDefinition.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
        webhookDefinition.AddChild(
            WebhooksManagementPermissions.WebhookDefinition.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var subscription = group.AddPermission(
            WebhooksManagementPermissions.WebhookSubscription.Default,
            L("Permission:Subscriptions"),
            MultiTenancySides.Host);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Update,
            L("Permission:Update"),
            MultiTenancySides.Host);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var sendAttempts = group.AddPermission(
            WebhooksManagementPermissions.WebhooksSendAttempts.Default,
            L("Permission:SendAttempts"),
            MultiTenancySides.Host);
        sendAttempts.AddChild(
            WebhooksManagementPermissions.WebhooksSendAttempts.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);
        sendAttempts.AddChild(
            WebhooksManagementPermissions.WebhooksSendAttempts.Resend,
            L("Permission:Resend"),
            MultiTenancySides.Host);

        group.AddPermission(
            WebhooksManagementPermissions.Publish,
            L("Permission:Publish"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);

        group.AddPermission(
            WebhooksManagementPermissions.ManageSettings,
            L("Permission:ManageSettings"),
            MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebhooksManagementResource>(name);
    }
}

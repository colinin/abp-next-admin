using LINGYUN.Abp.ExceptionHandling.Notifications;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.Users;

namespace LY.MicroService.RealtimeMessage.Emailing;

public class RealtimeMessageTemplateProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(CreateEmailTemplate());

        ReplaceDefaultTemplatePath(context);
    }

    protected virtual void ReplaceDefaultTemplatePath(ITemplateDefinitionContext context)
    {
        var exceptionTemplate = context.GetOrNull(AbpExceptionHandlingNotificationNames.NotificationName);
        if (exceptionTemplate != null)
        {
            exceptionTemplate
                .WithScribanEngine()
                .WithVirtualFilePath("/Emailing/Templates/ExceptionNotifier", isInlineLocalized: false)
                .Layout = "EmailNotifierLayout";
        }

        var tenantRegisterdTemplate = context.GetOrNull(TenantNotificationNames.NewTenantRegistered);
        if (tenantRegisterdTemplate != null)
        {
            tenantRegisterdTemplate
                .WithScribanEngine()
                .WithVirtualFilePath("/Emailing/Templates/NewTenantRegisterd", isInlineLocalized: false)
                .Layout = "EmailNotifierLayout";
        }

        var welcomeToApplicationTemplate = context.GetOrNull(UserNotificationNames.WelcomeToApplication);
        if (welcomeToApplicationTemplate != null)
        {
            welcomeToApplicationTemplate
                .WithScribanEngine()
                .WithVirtualFilePath("/Emailing/Templates/WelcomeToApplication", isInlineLocalized: false)
                .Layout = "EmailNotifierLayout";
        }
    }

    protected virtual TemplateDefinition[] CreateEmailTemplate()
    {
        return new TemplateDefinition[]
        {
            new TemplateDefinition(
                name: "EmailNotifierLayout",
                defaultCultureName: "en",
                isLayout: true)
            .WithScribanEngine()
            .WithVirtualFilePath( "/Emailing/Templates/layout.tpl", isInlineLocalized: true),
        };
    }
}

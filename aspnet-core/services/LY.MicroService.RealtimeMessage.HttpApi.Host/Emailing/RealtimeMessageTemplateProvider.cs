using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Scriban;

namespace LY.MicroService.RealtimeMessage.Emailing;

public class RealtimeMessageTemplateProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(CreateEmailTemplate());
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
                .WithVirtualFilePath(
                    "/Emailing/Templates/layout.tpl",
                    isInlineLocalized: false),
            new TemplateDefinition(
                name: "ExceptionNotifier", 
                defaultCultureName: "en", 
                layout: "EmailNotifierLayout")
                .WithScribanEngine()
                .WithVirtualFilePath(
                    "/Emailing/Templates/ExceptionNotifier",
                    isInlineLocalized: false),
            new TemplateDefinition(
                "NewTenantRegisterd",
                defaultCultureName: "en",
                layout: "EmailNotifierLayout")
                .WithScribanEngine()
                .WithVirtualFilePath(
                    "/Emailing/Templates/NewTenantRegisterd",
                    isInlineLocalized: false),
            new TemplateDefinition(
                "WelcomeToApplication",
                defaultCultureName: "en",
                layout: "EmailNotifierLayout")
                .WithScribanEngine()
                .WithVirtualFilePath(
                    "/Emailing/Templates/WelcomeToApplication",
                    isInlineLocalized: false),
        };
    }
}

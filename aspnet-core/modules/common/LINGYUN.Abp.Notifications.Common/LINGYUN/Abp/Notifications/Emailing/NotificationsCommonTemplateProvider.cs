using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

public class NotificationsCommonTemplateProvider : TemplateDefinitionProvider
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
                .WithVirtualFilePath( "/LINGYUN/Abp/Notifications/Emailing/Templates/layout.tpl", isInlineLocalized: true)
        };
    }
}

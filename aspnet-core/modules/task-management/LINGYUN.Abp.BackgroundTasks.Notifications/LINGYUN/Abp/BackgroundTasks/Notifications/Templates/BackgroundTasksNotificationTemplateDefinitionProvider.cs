using LINGYUN.Abp.BackgroundTasks.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.BackgroundTasks.Notifications.Templates;
public class BackgroundTasksNotificationTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(GetTemplateDefinitions());
    }

    private static TemplateDefinition[] GetTemplateDefinitions()
    {
        return new[]
        {
                new TemplateDefinition(
                   BackgroundTasksNotificationTemplates.JobExecutedNotification,
                   displayName: L("TextTemplate:JobExecutedNotification"),
                   localizationResource: typeof(BackgroundTasksResource)
                ).WithVirtualFilePath(
                    "/LINGYUN/Abp/BackgroundTasks/Notifications/Templates/JobExecutedNotification.tpl",
                    isInlineLocalized: true)
            };
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<BackgroundTasksResource>(name);
    }
}

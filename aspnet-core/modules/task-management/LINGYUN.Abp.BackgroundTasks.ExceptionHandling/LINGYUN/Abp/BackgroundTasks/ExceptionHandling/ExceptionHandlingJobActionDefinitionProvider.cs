using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.BackgroundTasks.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

public class ExceptionHandlingJobActionDefinitionProvider : JobActionDefinitionProvider
{
    public override void Define(IJobActionDefinitionContext context)
    {
        context.Add(
            new JobActionDefinition(
                JobExecutedFailedProvider.Name,
                JobActionType.Failed,
                L("JobExceptionNotifier"),
                JobExecutedFailedProvider.Paramters,
                L("JobExceptionNotifier"))
            .WithProvider<JobExecutedFailedProvider>());
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<BackgroundTasksResource>(name);
    }
}

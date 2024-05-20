using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks.Activities;
public class TestJobActionDefinitionProvider : JobActionDefinitionProvider
{
    public override void Define(IJobActionDefinitionContext context)
    {
        context.Add(new JobActionDefinition(
            "test_app",
            JobActionType.Failed,
            new FixedLocalizableString("test_app"),
            new List<JobActionParamter>())
        .WithExceptioinType(JobExceptionType.Application));

        context.Add(new JobActionDefinition(
            "test_net",
            JobActionType.Failed,
            new FixedLocalizableString("test_net"),
            new List<JobActionParamter>())
        .WithExceptioinType(JobExceptionType.Network));

        context.Add(new JobActionDefinition(
           "test_all",
           JobActionType.Failed,
           new FixedLocalizableString("test_all"),
           new List<JobActionParamter>())
        .WithExceptioinType(JobExceptionType.All));

        context.Add(new JobActionDefinition(
           "test_other",
           JobActionType.Failed,
           new FixedLocalizableString("test_other"),
           new List<JobActionParamter>()));
    }
}

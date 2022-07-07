namespace LINGYUN.Abp.BackgroundTasks.Activities;

public interface IJobActionDefinitionProvider
{
    void Define(IJobActionDefinitionContext context);
}

using LINGYUN.Abp.TaskManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.TaskManagement.Permissions;

public class TaskManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            TaskManagementPermissions.GroupName,
            L("Permissions:TaskManagement"));

        var backgroundJobs = group.AddPermission(
            TaskManagementPermissions.BackgroundJobs.Default,
            L("Permissions:BackgroundJobs"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Create,
            L("Permissions:CreateJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Update,
            L("Permissions:UpdateJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Delete,
            L("Permissions:DeleteJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Trigger,
            L("Permissions:TriggerJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Pause,
            L("Permissions:PauseJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Resume,
            L("Permissions:ResumeJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Start,
            L("Permissions:StartJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Stop,
            L("Permissions:StopJob"));
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.ManageSystemJobs,
            L("Permissions:ManageSystemJobs"));

        var backgroundJobLogs = group.AddPermission(
            TaskManagementPermissions.BackgroundJobLogs.Default,
            L("Permissions:BackgroundJobLogs"));
        backgroundJobLogs.AddChild(
            TaskManagementPermissions.BackgroundJobLogs.Delete,
            L("Permissions:DeleteJobLogs"));
    }

    private ILocalizableString L(string name)
    {
        return LocalizableString.Create<TaskManagementResource>(name);
    }
}

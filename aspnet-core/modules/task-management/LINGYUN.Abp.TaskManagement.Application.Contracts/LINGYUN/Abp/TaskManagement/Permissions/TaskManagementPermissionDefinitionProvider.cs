using LINGYUN.Abp.TaskManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TaskManagement.Permissions;

public class TaskManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            TaskManagementPermissions.GroupName,
            L("Permissions:TaskManagement"),
            MultiTenancySides.Host);

        var backgroundJobs = group.AddPermission(
            TaskManagementPermissions.BackgroundJobs.Default,
            L("Permissions:BackgroundJobs"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Create,
            L("Permissions:CreateJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Update,
            L("Permissions:UpdateJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Delete,
            L("Permissions:DeleteJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Trigger,
            L("Permissions:TriggerJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Pause,
            L("Permissions:PauseJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Resume,
            L("Permissions:ResumeJob"),
            MultiTenancySides.Host);
        backgroundJobs.AddChild(
            TaskManagementPermissions.BackgroundJobs.Stop,
            L("Permissions:StopJob"),
            MultiTenancySides.Host);

        var backgroundJobLogs = group.AddPermission(
            TaskManagementPermissions.BackgroundJobLogs.Default,
            L("Permissions:BackgroundJobLogs"),
            MultiTenancySides.Host);
        backgroundJobLogs.AddChild(
            TaskManagementPermissions.BackgroundJobLogs.Delete,
            L("Permissions:DeleteJobLogs"),
            MultiTenancySides.Host);
    }

    private ILocalizableString L(string name)
    {
        return LocalizableString.Create<TaskManagementResource>(name);
    }
}

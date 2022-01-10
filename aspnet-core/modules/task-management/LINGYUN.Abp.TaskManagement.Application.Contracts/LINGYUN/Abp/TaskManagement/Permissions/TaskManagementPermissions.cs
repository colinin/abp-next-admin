namespace LINGYUN.Abp.TaskManagement.Permissions;

public static class TaskManagementPermissions
{
    public const string GroupName = "TaskManagement";

    public static class BackgroundJobs
    {
        public const string Default = GroupName + ".BackgroundJobs";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Trigger = Default + ".Trigger";
        public const string Pause = Default + ".Pause";
        public const string Resume = Default + ".Resume";
        public const string Stop = Default + ".Stop";
    }

    public static class BackgroundJobLogs
    {
        public const string Default = GroupName + ".BackgroundJobLogs";
        public const string Delete = Default + ".Delete";
    }
}

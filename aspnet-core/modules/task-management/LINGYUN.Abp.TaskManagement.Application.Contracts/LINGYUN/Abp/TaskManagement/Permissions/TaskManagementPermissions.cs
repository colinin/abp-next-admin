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
        public const string Start = Default + ".Start";
        public const string Stop = Default + ".Stop";
        /// <summary>
        /// 是否允许管理系统内置作业
        /// 通常内置作业是由程序运行中产生，需要控制用户行为
        /// </summary>
        public const string ManageSystemJobs = Default + ".ManageSystemJobs";
    }

    public static class BackgroundJobLogs
    {
        public const string Default = GroupName + ".BackgroundJobLogs";
        public const string Delete = Default + ".Delete";
    }
}

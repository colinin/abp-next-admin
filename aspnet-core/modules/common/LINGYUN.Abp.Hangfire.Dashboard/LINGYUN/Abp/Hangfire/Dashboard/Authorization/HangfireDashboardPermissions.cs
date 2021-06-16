namespace LINGYUN.Abp.Hangfire.Dashboard.Authorization
{
    public static class HangfireDashboardPermissions
    {
        public const string GroupName = "Hangfire";

        public static class Dashboard
        {
            public const string Default = GroupName + ".Dashboard";

            public const string ManageJobs = Default + ".ManageJobs";
            // TODO: other pages...
        }
    }
}

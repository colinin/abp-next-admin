namespace LINGYUN.Abp.WorkflowManagement.Authorization
{
    public static class WorkflowManagementPermissions
    {
        public const string GroupName = "WorkflowManagement";

        public const string ManageSettings = GroupName + ".ManageSettings";

        public static class Engine
        {
            public const string Default = GroupName + ".Engine";

            public const string Initialize = Default + ".Initialize";

            public const string Register = Default + ".Register";
        }

        public static class WorkflowDef
        {
            public const string Default = GroupName + ".WorkflowDef";

            public const string Create = Default + ".Create";

            public const string Delete = Default + ".Delete";
        }
    }
}

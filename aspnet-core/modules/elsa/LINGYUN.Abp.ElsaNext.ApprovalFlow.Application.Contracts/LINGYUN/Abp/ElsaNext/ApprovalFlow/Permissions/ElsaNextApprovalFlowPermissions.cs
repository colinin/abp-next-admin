namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;

public static class ElsaNextApprovalFlowPermissions
{
    public const string GroupName = "ApprovalFlow";

    public static class Templates
    {
        public const string Default = GroupName + ".Templates";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Publish = Default + ".Publish";
    }

    public static class Instances
    {
        public const string Default = GroupName + ".Instances";
        public const string Create = Default + ".Create";
        public const string Decide = Default + ".Decide";
        public const string Withdraw = Default + ".Withdraw";
        public const string Manage = Default + ".Manage";
    }

    public static class FormDefinitions
    {
        public const string GroupName = "FormDefinitions";

        public static class Definitions
        {
            public const string Default = GroupName + ".Definitions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Publish = Default + ".Publish";
        }

        public static class Data
        {
            public const string Default = GroupName + ".Data";
            public const string Edit = Default + ".Edit";
        }
    }
}

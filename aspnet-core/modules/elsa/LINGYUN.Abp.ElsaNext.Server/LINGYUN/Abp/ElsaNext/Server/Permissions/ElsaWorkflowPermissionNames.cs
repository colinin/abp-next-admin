using LINGYUN.Abp.ElsaNext.Permissions;

namespace LINGYUN.Abp.ElsaNext.Server.Permissions;

public static class ElsaWorkflowPermissionNames
{
    public const string Prefix = ElsaPermissionNames.GroupName + ".Workflows";

    public static class Definitions
    {
        public const string Default = Prefix + ".Definitions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Execute = Default + ".Execute";
        public const string Publish = Default + ".Publish";
        public const string Retract = Default + ".Retract";
        public const string Refresh = Default + ".Refresh";
        public const string Reload = Default + ".Reload";
        public static class Versions
        {
            public const string Default = Definitions.Default + ".Versions";
            public const string Delete = Default + ".Delete";
            public const string Revert = Default + ".Revert";
        }
    }
    public static class Instances
    {
        public const string Default = Prefix + ".Instances";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Cancel = Default + ".Cancel";
        public static class Variables
        {
            public const string Default = Instances.Default + ".Variables";
            public const string Update = Default + ".Update";
        }
    }
}

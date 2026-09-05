using LINGYUN.Abp.ElsaNext.Permissions;

namespace LINGYUN.Abp.ElsaNext.Server.Permissions;

public static class ElsaWorkflowPermissions
{
    public static class Definitions
    {
        public const string Default = "workflow-definitions";
        public const string Read = ElsaPermissionVerbs.Read + ":" + Default;
        public const string Write = ElsaPermissionVerbs.Write + ":" + Default;
        public const string Delete = ElsaPermissionVerbs.Delete + ":" + Default;
        public const string Execute = ElsaPermissionVerbs.Execute + ":" + Default;
        public const string Publish = ElsaPermissionVerbs.Publish + ":" + Default;
        public const string Retract = ElsaPermissionVerbs.Retract + ":" + Default;
        public const string Refresh = ElsaPermissionVerbs.Actions + ":" + Default + ":" + ElsaPermissionVerbs.Refresh;
        public const string Reload = ElsaPermissionVerbs.Reload + ":" + Default + ":" + ElsaPermissionVerbs.Reload;
    }
    public static class Instances
    {
        public const string Default = "workflow-instances";
        public const string Read = ElsaPermissionVerbs.Read + ":" + Default;
        public const string Write = ElsaPermissionVerbs.Write + ":" + Default;
        public const string Delete = ElsaPermissionVerbs.Delete + ":" + Default;
        public const string Cancel = ElsaPermissionVerbs.Cancel + ":" + Default;
    }
}

using VoloSettingManagementPermissions = Volo.Abp.SettingManagement.SettingManagementPermissions;

namespace LINGYUN.Abp.SettingManagement;

public static class SettingManagementPermissions
{
    public const string GroupName = VoloSettingManagementPermissions.GroupName;
    public static class Definition
    {
        public const string Default = GroupName + ".Definition";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string DeleteOrRestore = Default + ".DeleteOrRestore";
    }
}

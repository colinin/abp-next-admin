using System;

namespace LINGYUN.Abp.SettingManagement
{
    public class AbpSettingManagementPermissions
    {
        public const string GroupName = "AbpSettingManagement";

        public class Settings
        {
            public const string Default = GroupName + ".Settings";

            public const string Manager = GroupName + ".Manager";
        }
    }
}

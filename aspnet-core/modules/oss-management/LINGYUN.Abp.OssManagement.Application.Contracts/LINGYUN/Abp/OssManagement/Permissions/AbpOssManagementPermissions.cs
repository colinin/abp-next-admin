namespace LINGYUN.Abp.OssManagement.Permissions;

public class AbpOssManagementPermissions
{
    public const string GroupName = "AbpOssManagement";

    public class Container
    {
        public const string Default = GroupName + ".Container";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";
    }

    public class OssObject
    {
        public const string Default = GroupName + ".OssObject";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";

        public const string Download = Default + ".Download";
    }
}

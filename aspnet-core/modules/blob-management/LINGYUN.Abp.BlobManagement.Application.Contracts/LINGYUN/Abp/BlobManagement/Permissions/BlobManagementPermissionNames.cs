namespace LINGYUN.Abp.BlobManagement.Permissions;

public static class BlobManagementPermissionNames
{
    public const string GroupName = "AbpBlobManagement";

    public class BlobContainer
    {
        public const string Default = GroupName + ".BlobContainer";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";
    }

    public class Blob
    {
        public const string Default = GroupName + ".Blob";

        public const string Create = Default + ".Create";

        public const string Delete = Default + ".Delete";

        public const string Download = Default + ".Download";

        public const string ManagePermissions = Default + ".ManagePermissions";

        public static class Resources
        {
            public const string Name = "LINGYUN.Abp.BlobManagement.Blob";
            public const string Delete = Name + ".Delete";
            public const string View = Name + ".View";
        }
    }
}

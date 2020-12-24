namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerPermissions
    {
        public const string GroupName = "AbpIdentityServer";

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Clone = Default + ".Clone";
            public const string ManagePermissions = Default + ".ManagePermissions";
            public const string ManageClaims = Default + ".ManageClaims";
            public const string ManageSecrets = Default + ".ManageSecrets";
            public const string ManageProperties = Default + ".ManageProperties";
        }

        public static class ApiResources
        {
            public const string Default = GroupName + ".ApiResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageClaims = Default + ".ManageClaims";
            public const string ManageSecrets = Default + ".ManageSecrets";
            public const string ManageScopes = Default + ".ManageScopes";
            public const string ManageProperties = Default + ".ManageProperties";
        }

        public static class ApiScopes
        {
            public const string Default = GroupName + ".ApiScopes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageClaims = Default + ".ManageClaims";
            public const string ManageProperties = Default + ".ManageProperties";
        }

        public static class IdentityResources
        {
            public const string Default = GroupName + ".IdentityResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageClaims = Default + ".ManageClaims";
            public const string ManageProperties = Default + ".ManageProperties";
        }

        public static class Grants
        {
            public const string Default = GroupName + ".Grants";
            public const string Delete = Default + ".Delete";
        }
    }
}

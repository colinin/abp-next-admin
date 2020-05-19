namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerPermissions
    {
        public const string GroupName = "IdentityServer";

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Enabled = Default + ".Enabled";
            public const string Disabled = Default + ".Disabled";
            public const string ManagePermissions = Default + ".ManagePermissions";

            public static class Claims
            {
                public const string Default = Clients.Default + ".Claims";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class Secrets
            {
                public const string Default = Clients.Default + ".Secrets";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class Properties
            {
                public const string Default = Clients.Default + ".Properties";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }
        }

        public static class ApiResources
        {
            public const string Default = GroupName + ".ApiResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public static class Scope
            {
                public const string Default = ApiResources.Default + ".Scope";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class Secrets
            {
                public const string Default = ApiResources.Default + ".Secrets";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }
        }
    }
}

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

            public static class Claims
            {
                public const string Default = Clients.Default + ".Claims";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }
        }
    }
}

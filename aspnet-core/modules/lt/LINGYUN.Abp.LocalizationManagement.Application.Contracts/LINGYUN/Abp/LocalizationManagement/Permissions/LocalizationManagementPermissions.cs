namespace LINGYUN.Abp.LocalizationManagement.Permissions
{
    public static class LocalizationManagementPermissions
    {
        public const string GroupName = "LocalizationManagement";

        public static class Resource
        {
            public const string Default = GroupName + ".Resource";

            public const string Create = Default + ".Create";

            public const string Update = Default + ".Update";

            public const string Delete = Default + ".Delete";
        }

        public static class Language
        {
            public const string Default = GroupName + ".Language";

            public const string Create = Default + ".Create";

            public const string Update = Default + ".Update";

            public const string Delete = Default + ".Delete";
        }

        public static class Text
        {
            public const string Default = GroupName + ".Text";

            public const string Create = Default + ".Create";

            public const string Update = Default + ".Update";

            public const string Delete = Default + ".Delete";
        }
    }
}

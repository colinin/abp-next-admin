namespace LINGYUN.Abp.Demo.Permissions;
public static class DemoPermissions
{
    public const string GroupName = "Demo";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";

        public const string Export = Default + ".Export";
        public const string Import = Default + ".Import";
    }

    // *** ADDED a NEW NESTED CLASS ***
    public static class Authors
    {
        public const string Default = GroupName + ".Authors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}

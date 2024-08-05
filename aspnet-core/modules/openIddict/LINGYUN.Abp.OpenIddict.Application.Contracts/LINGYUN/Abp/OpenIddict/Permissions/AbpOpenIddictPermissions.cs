namespace LINGYUN.Abp.OpenIddict.Permissions;

public class AbpOpenIddictPermissions
{
    public const string GroupName = "AbpOpenIddict";

    public static class Applications
    {
        public const string Default = GroupName + ".Applications";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";
        public const string ManageSecret = Default + ".ManageSecret";
    }

    public static class Authorizations
    {
        public const string Default = GroupName + ".Authorizations";
        public const string Delete = Default + ".Delete";
    }

    public static class Scopes
    {
        public const string Default = GroupName + ".Scopes";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Tokens
    {
        public const string Default = GroupName + ".Tokens";
        public const string Delete = Default + ".Delete";
    }
}

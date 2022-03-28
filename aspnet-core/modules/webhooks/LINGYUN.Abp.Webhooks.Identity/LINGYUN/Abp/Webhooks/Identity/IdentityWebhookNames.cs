namespace LINGYUN.Abp.Webhooks.Identity;

public static class IdentityWebhookNames
{
    public const string GroupName = "abp.webhooks.identity";
    public static class IdentityRole
    {
        public const string Prefix = GroupName + ".roles";
        public const string Create = Prefix + ".create";
        public const string Update = Prefix + ".update";
        public const string Delete = Prefix + ".delete";
        public const string ChangeName = Prefix + ".change_name";
    }

    public static class IdentityUser
    {
        public const string Prefix = GroupName + ".users";
        public const string Create = Prefix + ".create";
        public const string Update = Prefix + ".update";
        public const string Delete = Prefix + ".delete";
    }
}

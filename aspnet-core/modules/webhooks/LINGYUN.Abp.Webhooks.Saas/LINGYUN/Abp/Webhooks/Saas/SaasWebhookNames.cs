namespace LINGYUN.Abp.Webhooks.Saas;

public static class SaasWebhookNames
{
    public const string GroupName = "abp.webhooks.saas";
    public static class Edition
    {
        public const string Prefix = GroupName + ".editions";
        public const string Create = Prefix + ".create";
        public const string Update = Prefix + ".update";
        public const string Delete = Prefix + ".delete";
    }

    public static class Tenant
    {
        public const string Prefix = GroupName + ".tenants";
        public const string Create = Prefix + ".create";
        public const string Update = Prefix + ".update";
        public const string Delete = Prefix + ".delete";
    }
}

namespace LINGYUN.Abp.EventBus.CAP
{
    public static class AbpCAPHeaders
    {
        public static string ClientId { get; set; } = "cap-abp-client-id";

        public static string UserId { get; set; } = "cap-abp-user-id";

        public static string TenantId { get; set; } = "cap-abp-tenant-id";

        public static string MessageId { get; set; } = "cap-abp-message-id";
    }
}

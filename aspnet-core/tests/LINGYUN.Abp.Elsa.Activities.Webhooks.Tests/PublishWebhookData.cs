namespace LINGYUN.Abp.Elsa.Activities.Webhooks.Tests;
internal static class PublishWebhookData
{
    public readonly static string Name = "Send by xUnit test";
    public readonly static object SendData = "Test data";
    public readonly static Guid? TenantId = Guid.NewGuid();
}

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookAvailableDto
{
    public string Name { get; set; } = default!;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
}

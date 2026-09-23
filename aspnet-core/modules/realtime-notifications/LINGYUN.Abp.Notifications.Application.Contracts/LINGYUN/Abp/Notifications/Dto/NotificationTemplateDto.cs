namespace LINGYUN.Abp.Notifications;

public class NotificationTemplateDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Culture { get; set; }
}

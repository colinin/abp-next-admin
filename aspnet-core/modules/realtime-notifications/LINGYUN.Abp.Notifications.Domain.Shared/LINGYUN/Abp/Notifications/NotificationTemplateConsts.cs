namespace LINGYUN.Abp.Notifications;

public static class NotificationTemplateConsts
{
    public static int MaxNameLength { get; set; } = 255;

    public static int MaxTitleLength { get; set; } = 255;

    public static int MaxContentLength { get; set; } = 1024 * 1024;

    public static int MaxDescriptionLength { get; set; } = 255;

    public static int MaxCultureLength { get; set; } = 30;
}

namespace LINGYUN.Abp.Notifications;

public static class NotificationDefinitionRecordConsts
{
    public static int MaxNameLength { get; set; } = NotificationConsts.MaxNameLength;
    public static int MaxDisplayNameLength { get; set; } = 255;
    public static int MaxDescriptionLength { get; set; } = 255;
    public static int MaxTemplateLength { get; set; } = 128;
    public static int MaxProvidersLength { get; set; } = 200;
}

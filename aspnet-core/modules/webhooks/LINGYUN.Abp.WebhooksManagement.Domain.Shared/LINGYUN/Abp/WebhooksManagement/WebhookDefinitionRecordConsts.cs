namespace LINGYUN.Abp.WebhooksManagement;

public static class WebhookDefinitionRecordConsts
{
    public static int MaxNameLength { get; set; } = 128;

    public static int MaxDisplayNameLength { get; set; } = 256;

    public static int MaxDescriptionLength { get; set; } = 256;

    public static int MaxProvidersLength { get; set; } = 128;

    public static int MaxRequiredFeaturesLength { get; set; } = 256;
}

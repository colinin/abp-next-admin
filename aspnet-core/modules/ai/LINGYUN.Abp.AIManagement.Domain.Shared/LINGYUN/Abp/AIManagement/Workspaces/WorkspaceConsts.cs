namespace LINGYUN.Abp.AIManagement.Workspaces;
public static class WorkspaceConsts
{
    public static int MaxNameLength { get; set; } = 64;
    public static int MaxProviderLength { get; set; } = 20;
    public static int MaxModelNameLength { get; set; } = 64;
    public static int MaxDisplayNameLength { get; set; } = 128;
    public static int MaxDescriptionLength { get; set; } = 128;
    public static int MaxApiKeyLength { get; set; } = 64;
    public static int MaxApiBaseUrlLength { get; set; } = 128;
    public static int MaxSystemPromptLength { get; set; } = 512;
    public static int MaxInstructionsLength { get; set; } = 512;
    public static int MaxStateCheckersLength { get; set; } = 256;
}

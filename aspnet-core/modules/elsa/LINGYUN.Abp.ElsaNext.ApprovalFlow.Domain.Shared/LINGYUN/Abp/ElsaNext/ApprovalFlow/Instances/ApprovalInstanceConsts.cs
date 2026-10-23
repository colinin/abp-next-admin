namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public static class ApprovalInstanceConsts
{
    public static int MaxTemplateNameLength { get; set; } = 256;
    public static int MaxTemplateCategoryLength { get; set; } = 128;
    public static int MaxTitleLength { get; set; } = 512;
    public static int MaxBusinessKeyLength { get; set; } = 256;
    public static int MaxOutcomeLength { get; set; } = 64;
    public static int MaxCurrentNodeIdLength { get; set; } = 128;
    public static int MaxCurrentNodeNameLength { get; set; } = 256;
    public static int MaxElsaWorkflowInstanceIdLength { get; set; } = 128;
    public static int MaxStatusLength { get; set; } = 32;
    public static int MaxParticipantProviderLength { get; set; } = 128;
    public static int MaxParticipantTypeLength { get; set; } = 32;
    public static int MaxParticipantIdLength { get; set; } = 128;
    public static int MaxParticipantDisplayNameLength { get; set; } = 256;
}
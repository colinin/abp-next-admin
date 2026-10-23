namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public static class ApprovalNodeTaskConsts
{
    public static int MaxNodeInstanceIdLength { get; set; } = 128;
    public static int MaxNodeIdLength { get; set; } = 128;
    public static int MaxNodeNameLength { get; set; } = 256;
    public static int MaxTaskIdLength { get; set; } = 128;
    public static int MaxSignModeLength { get; set; } = 32;
    public static int MaxStatusLength { get; set; } = 32;
    public static int MaxOriginLength { get; set; } = 32;
    public static int MaxApproverProviderLength { get; set; } = 128;
    public static int MaxApproverTypeLength { get; set; } = 32;
    public static int MaxApproverIdLength { get; set; } = 128;
    public static int MaxApproverDisplayNameLength { get; set; } = 256;
    public static int MaxTitleLength { get; set; } = 200;
}
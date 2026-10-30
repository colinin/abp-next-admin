namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

public static class ApprovalRecordConsts
{
    public static int MaxActionKeyLength { get; set; } = 64;
    public static int MaxActionLabelLength { get; set; } = 256;
    public static int MaxActionEffectLength { get; set; } = 32;
    public static int MaxCommentLength { get; set; } = 2000;
    public static int MaxNodeIdLength { get; set; } = 128;
    public static int MaxNodeNameLength { get; set; } = 256;
    public static int MaxNodeInstanceIdLength { get; set; } = 128;
    public static int MaxTaskIdLength { get; set; } = 128;
    public static int MaxActorProviderLength { get; set; } = 128;
    public static int MaxActorTypeLength { get; set; } = 32;
    public static int MaxActorIdLength { get; set; } = 128;
    public static int MaxActorDisplayNameLength { get; set; } = 256;
}
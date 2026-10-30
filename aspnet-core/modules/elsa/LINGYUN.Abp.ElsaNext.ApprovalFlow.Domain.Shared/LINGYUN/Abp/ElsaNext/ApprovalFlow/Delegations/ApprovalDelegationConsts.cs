namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;

public static class ApprovalDelegationConsts
{
    public static int MaxParticipantProviderLength { get; set; } = 128;
    public static int MaxParticipantTypeLength { get; set; } = 32;
    public static int MaxParticipantIdLength { get; set; } = 128;
    public static int MaxParticipantDisplayNameLength { get; set; } = 256;
    public static int MaxRemarkLength { get; set; } = 1024;
}
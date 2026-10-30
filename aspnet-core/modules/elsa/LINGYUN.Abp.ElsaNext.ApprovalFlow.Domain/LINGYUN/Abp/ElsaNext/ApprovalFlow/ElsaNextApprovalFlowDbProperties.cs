namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

public static class ElsaNextApprovalFlowDbProperties
{
	public static string DbTablePrefix { get; set; } = "EX_";

	public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ElsaNextApprovalFlow";
}

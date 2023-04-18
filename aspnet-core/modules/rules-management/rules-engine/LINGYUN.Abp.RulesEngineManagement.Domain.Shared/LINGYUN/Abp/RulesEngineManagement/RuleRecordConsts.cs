namespace LINGYUN.Abp.RulesEngineManagement;
public static class RuleRecordConsts
{
    public static int MaxNameLength { get; set; } = 64;
    public static int MaxOperatorLength { get; set; } = 30;
    public static int MaxErrorMessageLength { get; set; } = 255;
    public static int MaxInjectWorkflowsLength { get; set; } = (MaxNameLength + 1) * 5;
    public static int MaxExpressionLength { get; set; } = int.MaxValue;
    public static int MaxSuccessEventLength { get; set; } = 128;
}

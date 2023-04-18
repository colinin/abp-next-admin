namespace LINGYUN.Abp.RulesEngineManagement;
public static class WorkflowRecordConsts
{
    public static int MaxNameLength { get; set; } = 64;
    public static int MaxTypeFullNameLength { get; set; } = 255;
    public static int MaxInjectWorkflowsLength { get; set; } = (MaxNameLength + 1) * 5;
}

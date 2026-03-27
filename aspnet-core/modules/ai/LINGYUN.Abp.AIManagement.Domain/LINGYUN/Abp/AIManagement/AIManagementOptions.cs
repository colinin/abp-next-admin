namespace LINGYUN.Abp.AIManagement;
public class AIManagementOptions
{
    public bool IsDynamicWorkspaceStoreEnabled {  get; set; }
    public bool SaveStaticWorkspacesToDatabase { get; set; }
    public bool IsDynamicAIToolStoreEnabled { get; set; }
    public bool SaveStaticAIToolsToDatabase { get; set; }
    public AIManagementOptions()
    {

    }
}

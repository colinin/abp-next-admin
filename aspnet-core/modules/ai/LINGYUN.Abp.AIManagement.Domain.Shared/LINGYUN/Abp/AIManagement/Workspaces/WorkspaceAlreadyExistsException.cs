using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceAlreadyExistsException : BusinessException
{
    public string Workspace { get; }
    public WorkspaceAlreadyExistsException(string workspace)
        : base(
            AIManagementErrorCodes.Workspace.NameAlreadyExists, 
            $"A Workspace named {workspace} already exists!")
    {
        Workspace = workspace;

        WithData(nameof(Workspace), workspace);
    }
}

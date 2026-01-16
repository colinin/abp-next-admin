using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public interface IWorkspaceDefinitionRecordRepository : IBasicRepository<WorkspaceDefinitionRecord, Guid>
{
    Task<WorkspaceDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}

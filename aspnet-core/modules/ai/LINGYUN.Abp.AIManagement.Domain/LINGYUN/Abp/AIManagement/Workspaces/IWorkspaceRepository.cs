using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public interface IWorkspaceRepository : IBasicRepository<Workspace, Guid>
{
    Task<Workspace?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}

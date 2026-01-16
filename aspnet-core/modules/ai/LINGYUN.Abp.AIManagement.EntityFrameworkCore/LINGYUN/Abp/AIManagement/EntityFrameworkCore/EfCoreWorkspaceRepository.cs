using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;
public class EfCoreWorkspaceRepository : EfCoreRepository<IAIManagementDbContext, Workspace, Guid>, IWorkspaceRepository
{
    public EfCoreWorkspaceRepository(
        IDbContextProvider<IAIManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<Workspace?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken)); ;
    }
}

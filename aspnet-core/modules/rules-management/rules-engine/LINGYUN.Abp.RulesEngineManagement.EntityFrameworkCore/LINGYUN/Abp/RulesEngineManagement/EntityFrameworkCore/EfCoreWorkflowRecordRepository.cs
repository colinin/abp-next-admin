using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;
public class EfCoreWorkflowRecordRepository : EfCoreRepository<IRulesEngineManagementDbContext, WorkflowRecord, Guid>, IWorkflowRecordRepository
{
    public EfCoreWorkflowRecordRepository(
        IDbContextProvider<IRulesEngineManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<WorkflowRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<WorkflowRecord>> GetListAsync(string typeFullName, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.TypeFullName == typeFullName)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ParamRecord>> GetParamsAsync(Guid workflowId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var paramQuery = from param in dbContext.Set<ParamRecord>()
                         join workflowParam in dbContext.Set<WorkflowParamRecord>()
                             on param.Id equals workflowParam.ParamId
                         where workflowParam.WorkflowId == workflowId
                         select param;

        return await paramQuery
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}

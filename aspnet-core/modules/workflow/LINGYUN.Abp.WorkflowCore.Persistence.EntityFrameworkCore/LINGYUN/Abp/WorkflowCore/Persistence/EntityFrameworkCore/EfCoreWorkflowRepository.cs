using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class EfCoreWorkflowRepository : EfCoreRepository<WorkflowDbContext, PersistedWorkflow, Guid>, IWorkflowRepository
    {
        public EfCoreWorkflowRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<PersistedWorkflow>> GetListAsync(
            WorkflowStatus? status,
            string type,
            DateTime? createdFrom,
            DateTime? createdTo,
            int skip,
            int take,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Include(x => x.ExecutionPointers)
                .WhereIf(status.HasValue, x => x.Status == status.Value)
                .WhereIf(!type.IsNullOrWhiteSpace(), x => x.WorkflowDefinitionId.Equals(type))
                .WhereIf(createdFrom.HasValue, x => x.CreationTime >= createdFrom.Value)
                .WhereIf(createdTo.HasValue, x => x.CreationTime <= createdTo.Value)
                .PageBy(skip, take)
                .ToListAsync();
        }

        public override async Task<IQueryable<PersistedWorkflow>> WithDetailsAsync()
        {
            var quertable = await base.WithDetailsAsync();
            return quertable
                .Include(x => x.ExecutionPointers)
                    .ThenInclude(p => p.ExtensionAttributes);
        }
    }
}

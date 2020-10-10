using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.RulesManagement
{
    public interface IEntityRuleGroupRepository : IBasicRepository<EntityRuleGroup, Guid>
    {
        Task<EntityRuleGroup> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            );
        Task<List<EntityRuleGroup>> GetListByTypeAsync(
            string entityFullTypeName,
            string sorting = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<EntityRule>> GetRulesAsync(
            Guid groupId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);


        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default);

        Task<List<EntityRuleGroup>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 1, 
            int maxResultCount = 10,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}

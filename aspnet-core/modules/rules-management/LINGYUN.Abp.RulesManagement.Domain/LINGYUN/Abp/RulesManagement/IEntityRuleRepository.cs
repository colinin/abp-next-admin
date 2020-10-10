using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.RulesManagement
{
    public interface IEntityRuleRepository : IBasicRepository<EntityRule, Guid>
    {
        Task<List<string>> GetInjectRuleNamesAsync(
            Guid ruleId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            );

        Task<List<EntityRule>> GetInjectRulesAsync(
            Guid ruleId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            );

        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default);

        Task<List<EntityRule>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 1,
            int maxResultCount = 10,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}

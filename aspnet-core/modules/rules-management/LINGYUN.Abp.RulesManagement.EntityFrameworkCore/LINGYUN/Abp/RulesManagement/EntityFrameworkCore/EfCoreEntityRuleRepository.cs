using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    public class EfCoreEntityRuleRepository : EfCoreRepository<IRulesManagementDbContext, EntityRule, Guid>, IEntityRuleRepository
    {
        public EfCoreEntityRuleRepository(
            IDbContextProvider<IRulesManagementDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async virtual Task<List<EntityRule>> GetInjectRulesAsync(
            Guid ruleId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            )
        {
            var query = from injectRule in DbContext.Set<EntityRuleInject>()
                        join rule in DbContext.EntityRules.IncludeDetails(includeDetails) on injectRule.InjectId equals rule.Id
                        where injectRule.RuleId == ruleId
                        select rule;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<long> GetCountAsync(
            string filter = null, 
            CancellationToken cancellationToken = default)
        {
            return await this
                .WhereIf(
                       !filter.IsNullOrWhiteSpace(),
                       rg =>
                           rg.Name.Contains(filter) ||
                           rg.DisplayName.Contains(filter) ||
                           rg.Operator.Contains(filter)
                   )
                   .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<EntityRule>> GetListAsync(
            string filter = null, 
            string sorting = null, 
            int skipCount = 1, 
            int maxResultCount = 10, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            return await this
                .WhereIf(
                       !filter.IsNullOrWhiteSpace(),
                       rg =>
                           rg.Name.Contains(filter) ||
                           rg.DisplayName.Contains(filter) ||
                           rg.Operator.Contains(filter)
                   )
                .OrderBy(sorting ?? nameof(EntityRule.CreationTime))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<EntityRule> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}

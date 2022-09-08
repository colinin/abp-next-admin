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
    public class EfCoreEntityRuleGroupRepository : EfCoreRepository<IRulesManagementDbContext, EntityRuleGroup, Guid>, IEntityRuleGroupRepository
    {
        public EfCoreEntityRuleGroupRepository(
            IDbContextProvider<IRulesManagementDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async virtual Task<EntityRuleGroup> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await this
                .Where(ug => ug.Name.Equals(name))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<EntityRule>> GetRulesAsync(
            Guid groupId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from ruleGroup in DbContext.Set<EntityRuleInGroup>()
                        join rule in DbContext.EntityRules.IncludeDetails(includeDetails) on ruleGroup.RuleId equals rule.Id
                        where ruleGroup.GroupId == groupId
                        select rule;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<long> GetCountAsync(
            string filter = null, 
            CancellationToken cancellationToken = default)
        {
            return await this.WhereIf(
                       !filter.IsNullOrWhiteSpace(),
                       rg =>
                           rg.Name.Contains(filter) ||
                           rg.DisplayName.Contains(filter) ||
                           rg.EntityFullTypeName.Contains(filter)
                   )
                   .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<EntityRuleGroup>> GetListByTypeAsync(
            string entityFullTypeName,
            string sorting = null,
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(ug => ug.EntityFullTypeName.Equals(entityFullTypeName))
                .OrderBy(sorting ?? nameof(EntityRuleGroup.CreationTime))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<EntityRuleGroup>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 1, 
            int maxResultCount = 10,
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    rg =>
                        rg.Name.Contains(filter) ||
                        rg.DisplayName.Contains(filter) ||
                        rg.EntityFullTypeName.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(EntityRuleGroup.CreationTime))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<EntityRuleGroup> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}

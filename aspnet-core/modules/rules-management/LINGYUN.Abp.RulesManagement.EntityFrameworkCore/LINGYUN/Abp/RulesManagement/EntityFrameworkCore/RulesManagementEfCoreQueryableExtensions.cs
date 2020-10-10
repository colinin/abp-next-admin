using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    public static class RulesManagementEfCoreQueryableExtensions
    {
        public static IQueryable<EntityRuleGroup> IncludeDetails(this IQueryable<EntityRuleGroup> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Rules)
                .Include(x => x.InjectRules);
        }

        public static IQueryable<EntityRule> IncludeDetails(this IQueryable<EntityRule> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.InjectRules)
                .Include(x => x.Params);
        }
    }
}

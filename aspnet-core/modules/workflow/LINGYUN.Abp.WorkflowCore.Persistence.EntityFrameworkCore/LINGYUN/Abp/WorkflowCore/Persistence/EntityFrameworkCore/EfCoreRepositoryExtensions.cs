using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    public static class EfCoreRepositoryExtensions
    {
        public static IQueryable<PersistedWorkflow> IncludeIf(
            this IQueryable<PersistedWorkflow> quertable,
            bool includeDetails = true)
        {
            return !includeDetails ? quertable :
                quertable
                    .Include(x => x.ExecutionPointers)
                        .ThenInclude(p => p.ExtensionAttributes);
        }
    }
}

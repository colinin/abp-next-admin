using JetBrains.Annotations;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection
{
    public class EfCoreFakeProtectionObjectRepository :
        EfCoreDataProtectionRepository<AbpDataProtectionTestDbContext, FakeProtectionObject, int>,
        IFakeProtectionObjectRepository
    {
        public EfCoreFakeProtectionObjectRepository(
            [NotNull] IDbContextProvider<AbpDataProtectionTestDbContext> dbContextProvider, 
            [NotNull] IDataAuthorizationService dataAuthorizationService, 
            [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder) 
            : base(dbContextProvider, dataAuthorizationService, entityTypeFilterBuilder)
        {
        }

        [DisableDataProtected]
        public async virtual Task<List<FakeProtectionObject>> GetAllListAsync()
        {
            return await (await GetQueryableAsync())
                .ToListAsync();
        }
    }
}

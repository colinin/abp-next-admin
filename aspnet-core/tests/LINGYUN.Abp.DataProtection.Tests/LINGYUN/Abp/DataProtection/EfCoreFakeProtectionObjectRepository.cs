using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection
{
    public class EfCoreFakeProtectionObjectRepository :
        EfCoreRepository<AbpDataProtectionDbContext, FakeProtectionObject, int>,
        IFakeProtectionObjectRepository
    {
        public EfCoreFakeProtectionObjectRepository(
            IDbContextProvider<AbpDataProtectionDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}

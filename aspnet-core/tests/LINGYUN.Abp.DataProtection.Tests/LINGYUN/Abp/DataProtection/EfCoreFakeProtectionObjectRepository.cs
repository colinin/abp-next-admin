using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection
{
    public class EfCoreFakeProtectionObjectRepository :
        EfCoreDataProtectionRepositoryBase<FakeDataProtectedDbContext, FakeProtectionObject, int>,
        IFakeProtectionObjectRepository
    {
        public EfCoreFakeProtectionObjectRepository(
            IDbContextProvider<FakeDataProtectedDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}

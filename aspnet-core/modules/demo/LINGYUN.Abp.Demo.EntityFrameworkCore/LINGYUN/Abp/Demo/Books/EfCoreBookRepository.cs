using JetBrains.Annotations;
using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Demo.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Demo.Books;
public class EfCoreBookRepository : EfCoreDataProtectionRepository<DemoDbContext, Book, Guid>, IBookRepository
{
    public EfCoreBookRepository(
        [NotNull] IDbContextProvider<DemoDbContext> dbContextProvider, 
        [NotNull] IDataAuthorizationService dataAuthorizationService, 
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder) 
        : base(dbContextProvider, dataAuthorizationService, entityTypeFilterBuilder)
    {
    }
}

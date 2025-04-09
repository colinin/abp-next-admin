using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;

public class EfCoreSubjectStrategyRepository : EfCoreRepository<IAbpDataProtectionManagementDbContext, SubjectStrategy, Guid>, ISubjectStrategyRepository
{
    public EfCoreSubjectStrategyRepository(
        IDbContextProvider<IAbpDataProtectionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<SubjectStrategy> FindBySubjectAsync(
        string subjectName,
        string subjectId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.SubjectName == subjectName && x.SubjectId == subjectId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}

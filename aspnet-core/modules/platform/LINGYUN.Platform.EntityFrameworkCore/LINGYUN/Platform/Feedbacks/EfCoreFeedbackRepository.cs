using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Feedbacks;
public class EfCoreFeedbackRepository : EfCoreRepository<IPlatformDbContext, Feedback, Guid>, IFeedbackRepository
{
    public EfCoreFeedbackRepository(
        IDbContextProvider<IPlatformDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Feedback> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Feedback>> GetListAsync(
        ISpecification<Feedback> specification,
        string sorting = $"{nameof(Feedback.CreationTime)} DESC",
        int maxResultCount = 25, 
        int skipCount = 0, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(Feedback.CreationTime)} DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<Feedback>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync())
            .Include(x => x.Comments)
            .Include(x => x.Attachments);
    }
}

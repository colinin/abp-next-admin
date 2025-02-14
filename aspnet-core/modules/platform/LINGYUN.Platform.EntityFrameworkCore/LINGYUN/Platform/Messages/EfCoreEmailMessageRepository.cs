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

namespace LINGYUN.Platform.Messages;
public class EfCoreEmailMessageRepository : EfCoreRepository<IPlatformDbContext, EmailMessage, Guid>, IEmailMessageRepository
{
    public EfCoreEmailMessageRepository(
        IDbContextProvider<IPlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(ISpecification<EmailMessage> specification, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<EmailMessage>> GetListAsync(
        ISpecification<EmailMessage> specification,
        string sorting = $"{nameof(Message.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(Message.CreationTime)} DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<EmailMessage>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync())
            .Include(x => x.Attachments)
            .Include(x => x.Headers);
    }
}

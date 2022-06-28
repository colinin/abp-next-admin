using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TextTemplating.EntityFrameworkCore;

public class EfCoreTextTemplateRepository :
    EfCoreRepository<ITextTemplatingDbContext, TextTemplate, Guid>,
    ITextTemplateRepository,
    ITransientDependency
{
    public EfCoreTextTemplateRepository(
        IDbContextProvider<ITextTemplatingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<TextTemplate> FindByNameAsync(string name, string culture = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name.Equals(name) && x.Culture.Equals(culture))
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}

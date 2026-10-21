using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Tokens;

public class EfCoreMySqlOpenIddictTokenRepository : EfCoreOpenIddictTokenRepository
{
    public EfCoreMySqlOpenIddictTokenRepository(
        IDbContextProvider<IOpenIddictDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async override Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var tokens = await (from token in dbContext.Set<OpenIddictToken>()
                            join authorization in dbContext.Set<OpenIddictAuthorization>()
                                on token.AuthorizationId equals authorization.Id into tokenAuthorizations
                            from tokenAuthorization in tokenAuthorizations.DefaultIfEmpty()
                            where token.CreationDate < date
                            where (token.Status != OpenIddictConstants.Statuses.Inactive && token.Status != OpenIddictConstants.Statuses.Valid) ||
                                  (tokenAuthorization != null && tokenAuthorization.Status != OpenIddictConstants.Statuses.Valid) ||
                                  token.ExpirationDate < DateTime.UtcNow
                            select token)
                            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, cancellationToken: GetCancellationToken(cancellationToken));

        return tokens.Count;
    }
}

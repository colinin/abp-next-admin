using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Authorizations;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

[Authorize(AbpOpenIddictPermissions.Authorizations.Default)]
public class OpenIddictAuthorizationAppService : OpenIddictApplicationServiceBase, IOpenIddictAuthorizationAppService
{
    protected IRepository<OpenIddictAuthorization, Guid> Repository { get; }

    public OpenIddictAuthorizationAppService(
        IRepository<OpenIddictAuthorization, Guid> repository)
    {
        Repository = repository;
    }

    [Authorize(AbpOpenIddictPermissions.Authorizations.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        await Repository.DeleteAsync(id);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<OpenIddictAuthorizationDto> GetAsync(Guid id)
    {
        var authorization = await Repository.GetAsync(id);

        return authorization.ToDto(JsonSerializer);
    }

    public async virtual Task<PagedResultDto<OpenIddictAuthorizationDto>> GetListAsync(OpenIddictAuthorizationGetListInput input)
    {
        var queryable = await Repository.GetQueryableAsync();
        if (input.ClientId.HasValue)
        {
            queryable = queryable.Where(x => x.ApplicationId == input.ClientId);
        }
        if (input.BeginCreationTime.HasValue)
        {
            queryable = queryable.Where(x => x.CreationTime >= input.BeginCreationTime);
        }
        if (input.EndCreationTime.HasValue)
        {
            queryable = queryable.Where(x => x.CreationTime <= input.BeginCreationTime);
        }
        if (!input.Status.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.Status == input.Status);
        }
        if (!input.Type.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.Type == input.Type);
        }
        if (!input.Subject.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.Subject == input.Subject);
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.Subject.Contains(input.Filter) ||
                x.Status.Contains(input.Filter) || x.Type.Contains(input.Filter) ||
                x.Scopes.Contains(input.Filter) || x.Properties.Contains(input.Filter));
        }
        queryable = queryable
            .OrderBy(input.Sorting ?? $"{nameof(OpenIddictAuthorization.CreationTime)} DESC")
            .PageBy(input.SkipCount, input.MaxResultCount);

        var totalCount = await AsyncExecuter.CountAsync(queryable);
        var entites = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<OpenIddictAuthorizationDto>(totalCount,
            entites.Select(entity => entity.ToDto(JsonSerializer)).ToList());
    }
}

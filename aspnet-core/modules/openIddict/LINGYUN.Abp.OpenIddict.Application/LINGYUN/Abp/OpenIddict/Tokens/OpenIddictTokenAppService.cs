using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Tokens;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Authorize(AbpOpenIddictPermissions.Tokens.Default)]
public class OpenIddictTokenAppService : OpenIddictApplicationServiceBase, IOpenIddictTokenAppService
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IRepository<OpenIddictToken, Guid> _tokenRepository;

    public OpenIddictTokenAppService(
        IOpenIddictTokenManager tokenManager,
        IRepository<OpenIddictToken, Guid> tokenRepository)
    {
        _tokenManager = tokenManager;
        _tokenRepository = tokenRepository;
    }

    [Authorize(AbpOpenIddictPermissions.Tokens.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var token = await _tokenRepository.GetAsync(id);

        await _tokenManager.DeleteAsync(token.ToModel());
    }

    public async virtual Task<OpenIddictTokenDto> GetAsync(Guid id)
    {
        var token = await _tokenRepository.GetAsync(id);

        return token.ToDto();
    }

    public async virtual Task<PagedResultDto<OpenIddictTokenDto>> GetListAsync(OpenIddictTokenGetListInput input)
    {
        var queryable = await _tokenRepository.GetQueryableAsync();
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
        if (input.BeginExpirationDate.HasValue)
        {
            queryable = queryable.Where(x => x.ExpirationDate >= input.BeginCreationTime);
        }
        if (input.EndExpirationDate.HasValue)
        {
            queryable = queryable.Where(x => x.ExpirationDate <= input.BeginCreationTime);
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
        if (!input.ReferenceId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.ReferenceId == input.ReferenceId);
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(x => x.Subject.Contains(input.Filter) ||
                x.Status.Contains(input.Filter) || x.Type.Contains(input.Filter) ||
                x.Payload.Contains(input.Filter) || x.Properties.Contains(input.Filter) ||
                x.ReferenceId.Contains(input.ReferenceId));
        }

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(OpenIddictToken.CreationTime)} DESC";
        }

        queryable = queryable
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        var totalCount = await AsyncExecuter.CountAsync(queryable);
        var entites = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<OpenIddictTokenDto>(totalCount,
            entites.Select(entity => entity.ToDto()).ToList());
    }
}

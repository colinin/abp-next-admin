using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Authorize(AbpOpenIddictPermissions.Scopes.Default)]
public class OpenIddictScopeAppService : OpenIddictApplicationServiceBase, IOpenIddictScopeAppService
{
    private readonly IOpenIddictScopeManager _scopeManager;

    private readonly IOpenIddictScopeRepository _scoppeRepository;

    public OpenIddictScopeAppService(
        IOpenIddictScopeManager scopeManager,
        IOpenIddictScopeRepository scopeRepository)
    {
        _scopeManager = scopeManager;
        _scoppeRepository = scopeRepository;
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Create)]
    public async virtual Task<OpenIddictScopeDto> CreateAsync(OpenIddictScopeCreateDto input)
    {
        if (await _scoppeRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Scopes.NameExisted)
                .WithData(nameof(OpenIddictScope.Name), input.Name);
        }

        var scope = new OpenIddictScope(GuidGenerator.Create());

        scope = input.ToEntity(scope, JsonSerializer);

        scope = await _scoppeRepository.InsertAsync(scope);

        await CurrentUnitOfWork.SaveChangesAsync();

        return scope.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var scope = await _scoppeRepository.GetAsync(id);

        await _scopeManager.DeleteAsync(scope.ToModel());
    }

    public async virtual Task<OpenIddictScopeDto> GetAsync(Guid id)
    {
        var scope = await _scoppeRepository.GetAsync(id);

        return scope.ToDto(JsonSerializer);
    }

    public async virtual Task<PagedResultDto<OpenIddictScopeDto>> GetListAsync(OpenIddictScopeGetListInput input)
    {
        var totalCount = await _scoppeRepository.GetCountAsync(input.Filter);
        var entites = await _scoppeRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter);

        return new PagedResultDto<OpenIddictScopeDto>(totalCount,
            entites.Select(entity => entity.ToDto(JsonSerializer)).ToList());
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Update)]
    public async virtual Task<OpenIddictScopeDto> UpdateAsync(Guid id, OpenIddictScopeUpdateDto input)
    {
        var scope = await _scoppeRepository.GetAsync(id);

        if (!string.Equals(scope.Name, input.Name) && 
            await _scoppeRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Scopes.NameExisted)
                .WithData(nameof(OpenIddictScope.Name), input.Name);
        }

        scope.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        scope = input.ToEntity(scope, JsonSerializer);

        var cache = LazyServiceProvider.LazyGetRequiredService<IOpenIddictScopeCache<OpenIddictScopeModel>>();

        await cache.RemoveAsync(scope.ToModel(), GetCancellationToken());

        await _scoppeRepository.UpdateAsync(scope);

        return scope.ToDto(JsonSerializer);
    }
}

using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Authorize(AbpOpenIddictPermissions.Scopes.Default)]
public class OpenIddictScopeAppService : OpenIddictApplicationServiceBase, IOpenIddictScopeAppService
{
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IOpenIddictScopeRepository _scoppeRepository;
    private readonly AbpOpenIddictIdentifierConverter _identifierConverter;

    public OpenIddictScopeAppService(
        IOpenIddictScopeManager scopeManager,
        IOpenIddictScopeRepository scopeRepository,
        AbpOpenIddictIdentifierConverter identifierConverter)
    {
        _scopeManager = scopeManager;
        _scoppeRepository = scopeRepository;
        _identifierConverter = identifierConverter;
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Create)]
    public async virtual Task<OpenIddictScopeDto> CreateAsync(OpenIddictScopeCreateDto input)
    {
        if (await _scopeManager.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Scopes.NameExisted)
                .WithData(nameof(OpenIddictScope.Name), input.Name);
        }

        var scope = new OpenIddictScope(GuidGenerator.Create());

        scope = input.ToEntity(scope, JsonSerializer);

        await _scopeManager.CreateAsync(scope.ToModel());

        scope = await _scoppeRepository.FindByIdAsync(scope.Id);

        return scope.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var scope = await _scopeManager.FindByIdAsync(_identifierConverter.ToString(id));

        await _scopeManager.DeleteAsync(scope);
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

        await _scopeManager.UpdateAsync(scope.ToModel());

        scope = await _scoppeRepository.GetAsync(id);

        return scope.ToDto(JsonSerializer);
    }
}

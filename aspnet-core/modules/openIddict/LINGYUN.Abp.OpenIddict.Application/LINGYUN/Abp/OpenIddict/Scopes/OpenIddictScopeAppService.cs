using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.OpenIddict.Scopes;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Authorize(AbpOpenIddictPermissions.Scopes.Default)]
public class OpenIddictScopeAppService : OpenIddictApplicationServiceBase, IOpenIddictScopeAppService
{
    protected IOpenIddictScopeRepository Repository { get; }

    public OpenIddictScopeAppService(IOpenIddictScopeRepository repository)
    {
        Repository = repository;
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Create)]
    public async virtual Task<OpenIddictScopeDto> CreateAsync(OpenIddictScopeCreateDto input)
    {
        if (await Repository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Scopes.NameExisted)
                .WithData(nameof(OpenIddictScope.Name), input.Name);
        }

        var scope = new OpenIddictScope(GuidGenerator.Create());
        scope = input.ToEntity(scope, JsonSerializer);

        scope = await Repository.InsertAsync(scope);

        await CurrentUnitOfWork.SaveChangesAsync();

        return scope.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        await Repository.DeleteAsync(id);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<OpenIddictScopeDto> GetAsync(Guid id)
    {
        var scope = await Repository.GetAsync(id);

        return scope.ToDto(JsonSerializer);
    }

    public async virtual Task<PagedResultDto<OpenIddictScopeDto>> GetListAsync(OpenIddictScopeGetListInput input)
    {
        var totalCount = await Repository.GetCountAsync(input.Filter);
        var entites = await Repository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter);

        return new PagedResultDto<OpenIddictScopeDto>(totalCount,
            entites.Select(entity => entity.ToDto(JsonSerializer)).ToList());
    }

    [Authorize(AbpOpenIddictPermissions.Scopes.Update)]
    public async virtual Task<OpenIddictScopeDto> UpdateAsync(Guid id, OpenIddictScopeUpdateDto input)
    {
        var scope = await Repository.GetAsync(id);

        if (!string.Equals(scope.Name, input.Name) && 
            await Repository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Scopes.NameExisted)
                .WithData(nameof(OpenIddictScope.Name), input.Name);
        }

        scope = input.ToEntity(scope, JsonSerializer);

        scope = await Repository.UpdateAsync(scope);

        await CurrentUnitOfWork.SaveChangesAsync();

        return scope.ToDto(JsonSerializer);
    }
}

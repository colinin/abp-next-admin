using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.OpenIddict.Applications;

namespace LINGYUN.Abp.OpenIddict.Applications;

[Authorize(AbpOpenIddictPermissions.Applications.Default)]
public class OpenIddictApplicationAppService : OpenIddictApplicationServiceBase, IOpenIddictApplicationAppService
{
    protected IOpenIddictApplicationRepository Repository { get; }

    public OpenIddictApplicationAppService(
        IOpenIddictApplicationRepository repository)
    {
        Repository = repository;
    }

    public async virtual Task<OpenIddictApplicationDto> GetAsync(Guid id)
    {
        var application = await Repository.GetAsync(id);

        return application.ToDto(JsonSerializer);
    }

    public async virtual Task<PagedResultDto<OpenIddictApplicationDto>> GetListAsync(OpenIddictApplicationGetListInput input)
    {
        var totalCount = await Repository.GetCountAsync(input.Filter);
        var entites = await Repository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter);

        return new PagedResultDto<OpenIddictApplicationDto>(totalCount,
            entites.Select(entity => entity.ToDto(JsonSerializer)).ToList());
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Create)]
    public async virtual Task<OpenIddictApplicationDto> CreateAsync(OpenIddictApplicationCreateDto input)
    {
        if (await Repository.FindByClientIdAsync(input.ClientId) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Applications.ClientIdExisted)
                .WithData(nameof(OpenIddictApplication.ClientId), input.ClientId);
        }

        var application = new OpenIddictApplication(GuidGenerator.Create());
        application = input.ToEntity(application, JsonSerializer);

        application = await Repository.InsertAsync(application);

        await CurrentUnitOfWork.SaveChangesAsync();

        return application.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Update)]
    public async virtual Task<OpenIddictApplicationDto> UpdateAsync(Guid id, OpenIddictApplicationUpdateDto input)
    {
        var application = await Repository.GetAsync(id);

        if (!string.Equals(application.ClientId, input.ClientId) && 
            await Repository.FindByClientIdAsync(input.ClientId) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Applications.ClientIdExisted)
                .WithData(nameof(OpenIddictApplicationCreateDto.ClientId), input.ClientId);
        }

        application = input.ToEntity(application, JsonSerializer);

        application = await Repository.UpdateAsync(application);

        await CurrentUnitOfWork.SaveChangesAsync();

        return application.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        await Repository.DeleteAsync(id);

        await CurrentUnitOfWork.SaveChangesAsync();
    }
}

using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.OpenIddict.Applications;

namespace LINGYUN.Abp.OpenIddict.Applications;

[Authorize(AbpOpenIddictPermissions.Applications.Default)]
public class OpenIddictApplicationAppService : OpenIddictApplicationServiceBase, IOpenIddictApplicationAppService
{
    private readonly IAbpApplicationManager _applicationManager;
    private readonly IOpenIddictApplicationRepository _applicationRepository;

    public OpenIddictApplicationAppService(
        IAbpApplicationManager applicationManager,
        IOpenIddictApplicationRepository applicationRepository)
    {
        _applicationManager = applicationManager;
        _applicationRepository = applicationRepository;
    }

    public async virtual Task<OpenIddictApplicationDto> GetAsync(Guid id)
    {
        var application = await _applicationRepository.GetAsync(id);

        return application.ToDto(JsonSerializer);
    }

    public async virtual Task<PagedResultDto<OpenIddictApplicationDto>> GetListAsync(OpenIddictApplicationGetListInput input)
    {
        var totalCount = await _applicationRepository.GetCountAsync(input.Filter);
        var entites = await _applicationRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter);

        return new PagedResultDto<OpenIddictApplicationDto>(totalCount,
            entites.Select(entity => entity.ToDto(JsonSerializer)).ToList());
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Create)]
    public async virtual Task<OpenIddictApplicationDto> CreateAsync(OpenIddictApplicationCreateDto input)
    {
        if (await _applicationRepository.FindByClientIdAsync(input.ClientId) != null)
        {
            throw new BusinessException(OpenIddictApplicationErrorCodes.Applications.ClientIdExisted)
                .WithData(nameof(OpenIddictApplication.ClientId), input.ClientId);
        }

        var application = new OpenIddictApplication(GuidGenerator.Create())
        {
            ClientId = input.ClientId,
        };

        application = input.ToEntity(application, JsonSerializer);

        await _applicationManager.CreateAsync(application.ToModel(), input.ClientSecret);

        application = await _applicationRepository.FindByClientIdAsync(input.ClientId);

        return application.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Update)]
    public async virtual Task<OpenIddictApplicationDto> UpdateAsync(Guid id, OpenIddictApplicationUpdateDto input)
    {
        var application = await _applicationRepository.GetAsync(id);

        //if (!string.Equals(application.ClientId, input.ClientId) && 
        //    await _applicationRepository.FindByClientIdAsync(input.ClientId) != null)
        //{
        //    throw new BusinessException(OpenIddictApplicationErrorCodes.Applications.ClientIdExisted)
        //        .WithData(nameof(OpenIddictApplicationCreateDto.ClientId), input.ClientId);
        //}

        application.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        application = input.ToEntity(application, JsonSerializer);

        if (input.ClientSecret.IsNullOrWhiteSpace())
        {
            await _applicationManager.UpdateAsync(application.ToModel());
        }
        else
        {
            await _applicationManager.UpdateAsync(application.ToModel(), input.ClientSecret);
        }

        application = await _applicationRepository.FindAsync(id);

        return application.ToDto(JsonSerializer);
    }

    [Authorize(AbpOpenIddictPermissions.Applications.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var application = await _applicationRepository.GetAsync(id);
        await _applicationManager.DeleteAsync(application.ToModel());
    }
}

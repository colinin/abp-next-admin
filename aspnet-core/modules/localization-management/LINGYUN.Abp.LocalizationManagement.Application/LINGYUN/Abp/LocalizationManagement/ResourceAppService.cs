using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.LocalizationManagement;

[Authorize(LocalizationManagementPermissions.Resource.Default)]
public class ResourceAppService : LocalizationAppServiceBase, IResourceAppService
{
    private readonly IResourceRepository _repository;

    public ResourceAppService(IResourceRepository repository)
    {
        _repository = repository;
    }

    public async virtual Task<ResourceDto> GetByNameAsync(string name)
    {
        var resource = await InternalGetByNameAsync(name);

        return ObjectMapper.Map<Resource, ResourceDto>(resource);
    }

    [Authorize(LocalizationManagementPermissions.Resource.Create)]
    public async virtual Task<ResourceDto> CreateAsync(ResourceCreateDto input)
    {
        if (await _repository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(LocalizationErrorCodes.Resource.NameAlreadyExists)
                .WithData(nameof(Resource.Name), input.Name);
        }

        var resource = new Resource(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.Description,
            input.DefaultCultureName);

        resource = await _repository.InsertAsync(resource);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Resource, ResourceDto>(resource);
    }

    [Authorize(LocalizationManagementPermissions.Resource.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var resource = await InternalGetByNameAsync(name);

        await _repository.DeleteAsync(resource);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(LocalizationManagementPermissions.Resource.Update)]
    public async virtual Task<ResourceDto> UpdateAsync(string name, ResourceUpdateDto input)
    {
        var resource = await InternalGetByNameAsync(name);

        resource.SetDisplayName(input.DisplayName);
        resource.SetDescription(input.Description);
        resource.SetDefaultCultureName(input.DefaultCultureName);

        await _repository.UpdateAsync(resource);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Resource, ResourceDto>(resource);
    }

    private async Task<Resource> InternalGetByNameAsync(string name)
    {
        var resource = await _repository.FindByNameAsync(name);

        return resource ?? throw new BusinessException(LocalizationErrorCodes.Resource.NameNotFoundOrStaticNotAllowed)
                .WithData(nameof(Resource.Name), name);
    }
}

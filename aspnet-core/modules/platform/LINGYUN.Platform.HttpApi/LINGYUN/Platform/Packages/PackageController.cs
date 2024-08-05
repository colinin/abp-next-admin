using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace LINGYUN.Platform.Packages;

[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Area("platform")]
[Route("api/platform/packages")]
public class PackageController : PlatformControllerBase, IPackageAppService
{
    private readonly IPackageAppService _service;

    public PackageController(IPackageAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(PlatformPermissions.Package.Create)]
    public virtual Task<PackageDto> CreateAsync(PackageCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(PlatformPermissions.Package.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpPost]
    [Route("{id}/blob")]
    [Authorize(PlatformPermissions.Package.ManageBlobs)]
    public virtual Task<PackageBlobDto> UploadBlobAsync(
        Guid id,
        [FromForm] PackageBlobUploadDto input)
    {
        return _service.UploadBlobAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}/blob/{Name}")]
    [Authorize(PlatformPermissions.Package.ManageBlobs)]
    public virtual Task RemoveBlobAsync(
        Guid id,
        PackageBlobRemoveDto input)
    {
        return _service.RemoveBlobAsync(id, input);
    }

    [HttpGet]
    [Route("{id}/blob/{Name}")]
    [AllowAnonymous]
    public virtual Task<IRemoteStreamContent> DownloadBlobAsync(Guid id, PackageBlobDownloadInput input)
    {
        return _service.DownloadBlobAsync(id, input);
    }

    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
    public virtual Task<PackageDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("{Name}/latest")]
    [Route("{Name}/latest/{Version}")]
    [AllowAnonymous]
    public virtual Task<PackageDto> GetLatestAsync(PackageGetLatestInput input)
    {
        return _service.GetLatestAsync(input);
    }

    [HttpGet]
    [Authorize(PlatformPermissions.Package.Default)]
    public virtual Task<PagedResultDto<PackageDto>> GetListAsync(PackageGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(PlatformPermissions.Package.Update)]
    public virtual Task<PackageDto> UpdateAsync(Guid id, PackageUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}

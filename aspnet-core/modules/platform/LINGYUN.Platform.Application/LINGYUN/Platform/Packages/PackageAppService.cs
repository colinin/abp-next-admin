using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Data;

namespace LINGYUN.Platform.Packages;

public class PackageAppService : PlatformApplicationServiceBase, IPackageAppService
{
    private readonly IPackageBlobManager _blobManager;
    private readonly IPackageRepository _packageRepository;

    public PackageAppService(
        IPackageBlobManager blobManager, 
        IPackageRepository packageRepository)
    {
        _blobManager = blobManager;
        _packageRepository = packageRepository;
    }

    public async virtual Task<PackageDto> GetLatestAsync(PackageGetLatestInput input)
    {
        var package = await _packageRepository.FindLatestAsync(input.Name, input.Version);

        return ObjectMapper.Map<Package, PackageDto>(package);
    }

    [Authorize(PlatformPermissions.Package.Create)]
    public async virtual Task<PackageDto> CreateAsync(PackageCreateDto input)
    {
        var package = await _packageRepository.FindLatestAsync(input.Name);
        if (package != null)
        {
            if (string.Compare(input.Version, package.Version, StringComparison.CurrentCultureIgnoreCase) <= 0)
            {
                throw new BusinessException(PlatformErrorCodes.PackageVersionDegraded)
                    .WithData(nameof(Package.Name), input.Name)
                    .WithData(nameof(Package.Version), input.Version);
            }
        }

        package = new Package(
            GuidGenerator.Create(),
            input.Name,
            input.Note,
            input.Version,
            input.Description);

        UpdateByInput(package, input);

        package = await _packageRepository.InsertAsync(package);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Package, PackageDto>(package);
    }

    [Authorize(PlatformPermissions.Package.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        await _packageRepository.DeleteAsync(id);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(PlatformPermissions.Package.ManageBlobs)]
    public async virtual Task<PackageBlobDto> UploadBlobAsync(
        Guid id,
        PackageBlobUploadDto input)
    {
        var package = await _packageRepository.GetAsync(id);

        var packageBlob = package.FindBlob(input.Name);

        if (packageBlob == null)
        {
            packageBlob = package.CreateBlob(
                input.Name,
                input.CreatedAt,
                input.UpdatedAt,
                input.Size ?? input.File.ContentLength,
                input.Summary);

            await _blobManager.SaveBlobAsync(package, packageBlob, input.File.GetStream());
        }

        await _packageRepository.UpdateAsync(package);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<PackageBlob, PackageBlobDto>(packageBlob);
    }

    [Authorize(PlatformPermissions.Package.ManageBlobs)]
    public async virtual Task RemoveBlobAsync(
        Guid id,
        PackageBlobRemoveDto input)
    {
        var package = await _packageRepository.GetAsync(id);

        var packageBlob = package.FindBlob(input.Name);

        await _blobManager.RemoveBlobAsync(package, packageBlob);

        package.RemoveBlob(input.Name);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<IRemoteStreamContent> DownloadBlobAsync(Guid id, PackageBlobDownloadInput input)
    {
        var package = await _packageRepository.GetAsync(id);
        var packageBlob = package.FindBlob(input.Name);

        Stream stream;
        using (CurrentTenant.Change(null))
        {
            stream = await _blobManager.DownloadBlobAsync(package, packageBlob);
        }

        return new RemoteStreamContent(stream, packageBlob.Name, packageBlob.ContentType);
    }

    public async virtual Task<PackageDto> GetAsync(Guid id)
    {
        var package = await _packageRepository.GetAsync(id);

        return ObjectMapper.Map<Package, PackageDto>(package);
    }

    [Authorize(PlatformPermissions.Package.Default)]
    public async virtual Task<PagedResultDto<PackageDto>> GetListAsync(PackageGetPagedListInput input)
    {
        var filter = new PackageFilter(
            input.Filter, input.Name, input.Note, input.Version, 
            input.Description, input.ForceUpdate, input.Authors);

        var specification = new PackageSpecification(filter);

        var totalCount = await _packageRepository.GetCountAsync(specification);
        var entities = await _packageRepository.GetListAsync(specification);

        return new PagedResultDto<PackageDto>(
            totalCount,
            ObjectMapper.Map<List<Package>, List<PackageDto>>(entities));
    }

    [Authorize(PlatformPermissions.Package.Update)]
    public async virtual Task<PackageDto> UpdateAsync(Guid id, PackageUpdateDto input)
    {
        var package = await _packageRepository.GetAsync(id);

        UpdateByInput(package, input);

        package.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        package = await _packageRepository.UpdateAsync(package);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Package, PackageDto>(package);
    }

    protected virtual void UpdateByInput(Package entity, PackageCreateOrUpdateDto input)
    {
        if (!string.Equals(entity.Note, input.Note, StringComparison.CurrentCultureIgnoreCase))
        {
            entity.SetNote(input.Note);
        }

        if (!string.Equals(entity.Authors, input.Authors, StringComparison.CurrentCultureIgnoreCase))
        {
            entity.Authors = Check.Length(input.Authors, nameof(Package.Authors), PackageConsts.MaxAuthorsLength);
        }

        if (!string.Equals(entity.Description, input.Description, StringComparison.CurrentCultureIgnoreCase))
        {
            entity.Description = Check.Length(input.Description, nameof(Package.Description), PackageConsts.MaxDescriptionLength);
        }

        entity.ForceUpdate = input.ForceUpdate;
        entity.Level = input.Level;
    }
}

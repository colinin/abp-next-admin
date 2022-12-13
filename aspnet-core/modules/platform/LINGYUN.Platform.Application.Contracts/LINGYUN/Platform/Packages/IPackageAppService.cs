using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Platform.Packages;

public interface IPackageAppService :
    ICrudAppService<
        PackageDto,
        Guid,
        PackageGetPagedListInput,
        PackageCreateDto,
        PackageUpdateDto>
{
    Task<PackageDto> GetLatestAsync(PackageGetLatestInput input);

    Task<PackageBlobDto> UploadBlobAsync(
        Guid id,
        PackageBlobUploadDto input);

    Task RemoveBlobAsync(
        Guid id,
        PackageBlobRemoveDto input);

    Task<IRemoteStreamContent> DownloadBlobAsync(
        Guid id,
        PackageBlobDownloadInput input);
}

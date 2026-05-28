using LINGYUN.Abp.BlobManagement.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement;

public interface IPublicBlobAppService : IApplicationService
{
    Task<IRemoteStreamContent> DownloadAsync(Guid id);

    Task<IRemoteStreamContent> DownloadByNameAsync(string name);

    Task<BlobDto> GetAsync(Guid id);

    Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListWithoutContainerInput input);
}

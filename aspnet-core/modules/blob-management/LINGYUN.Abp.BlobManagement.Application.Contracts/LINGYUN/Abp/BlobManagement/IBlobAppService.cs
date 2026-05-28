using LINGYUN.Abp.BlobManagement.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobAppService : IApplicationService
{
    Task<BlobDto> CreateFileAsync(BlobFileCreateDto input);

    Task CreateChunkFileAsync(BlobFileChunkCreateDto input);

    Task<BlobDto> CreateFolderAsync(BlobFolderCreateDto input);

    Task DeleteAsync(Guid id);

    Task<string> GeneratePreviewUrlAsync(Guid id);

    Task<string> GenerateDownloadUrlAsync(Guid id);

    Task<IRemoteStreamContent> DownloadAsync(BlobDownloadByIdInput input);

    Task<IRemoteStreamContent> PreviewAsync(BlobDownloadByIdInput input);

    Task<IRemoteStreamContent> DownloadByNameAsync(BlobDownloadByNameInput input);

    Task<BlobDto> GetAsync(Guid id);

    Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListInput input);
}

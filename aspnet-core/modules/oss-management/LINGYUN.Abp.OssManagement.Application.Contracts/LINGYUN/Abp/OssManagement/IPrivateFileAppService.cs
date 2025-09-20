using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement;

public interface IPrivateFileAppService : IApplicationService
{
    Task<OssObjectDto> UploadAsync(UploadFileInput input);

    Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input);

    Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input);

    Task UploadChunkAsync(UploadFileChunkInput input);

    Task DeleteAsync(GetPublicFileInput input);

    Task<FileShareDto> ShareAsync(FileShareInput input);

    Task<ListResultDto<MyFileShareDto>> GetShareListAsync();
}

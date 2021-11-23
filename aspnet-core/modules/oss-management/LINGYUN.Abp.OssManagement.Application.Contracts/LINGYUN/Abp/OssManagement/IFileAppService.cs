using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement
{
    public interface IFileAppService : IApplicationService
    {
        Task<OssObjectDto> UploadAsync(UploadFileInput input);

        Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input);

        Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input);

        Task UploadAsync(UploadFileChunkInput input);

        Task DeleteAsync(GetPublicFileInput input);
    }
}

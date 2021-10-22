using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OssManagement
{
    public interface IFileAppService : IApplicationService
    {
        Task<OssObjectDto> UploadAsync(UploadFileInput input);

        Task<Stream> GetAsync(GetPublicFileInput input);

        Task UploadAsync(UploadFileChunkInput input);
    }
}

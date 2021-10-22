using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OssManagement
{
    public interface IFileAppService : IApplicationService
    {
        Task<OssObjectDto> UploadAsync(UploadPublicFileInput input);

        Task<Stream> GetAsync(GetPublicFileInput input);
    }
}

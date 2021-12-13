using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement
{
    public interface IOssObjectAppService : IApplicationService
    {
        Task<OssObjectDto> CreateAsync(CreateOssObjectInput input);

        Task<OssObjectDto> GetAsync(GetOssObjectInput input);

        Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input);

        Task DeleteAsync(GetOssObjectInput input);

        Task BulkDeleteAsync(BulkDeleteOssObjectInput input);
    }
}

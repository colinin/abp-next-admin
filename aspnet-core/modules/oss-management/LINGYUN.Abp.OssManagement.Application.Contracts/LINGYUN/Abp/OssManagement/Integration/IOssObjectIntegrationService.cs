using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement.Integration;

[IntegrationService]
public interface IOssObjectIntegrationService : IApplicationService
{
    Task<OssObjectDto> CreateAsync(CreateOssObjectInput input);

    Task DeleteAsync(GetOssObjectInput input);

    Task<GetOssObjectExistsResult> ExistsAsync(GetOssObjectInput input);

    Task<IRemoteStreamContent> GetAsync(GetOssObjectInput input);
}

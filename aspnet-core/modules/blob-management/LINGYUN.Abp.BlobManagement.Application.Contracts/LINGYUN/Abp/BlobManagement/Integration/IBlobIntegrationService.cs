using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Integration.Dtos;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement.Integration;

[IntegrationService]
public interface IBlobIntegrationService : IApplicationService
{
    Task<BlobDto> CreateAsync(BlobFileCreateIntegrationDto input);

    Task DeleteAsync(BlobFileGetByNameIntegrationDto input);

    Task<bool> ExistsAsync(BlobFileGetByNameIntegrationDto input);

    Task<IRemoteStreamContent> GetContentAsync(BlobFileGetByNameIntegrationDto input);
}

using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IGlobalConfigurationAppService : IApplicationService
    {
        Task<GlobalConfigurationDto> GetAsync(GlobalGetByAppIdInputDto globalGetByAppId);

        Task<GlobalConfigurationDto> CreateAsync(GlobalCreateDto globalCreateDto);

        Task<GlobalConfigurationDto> UpdateAsync(GlobalUpdateDto globalUpdateDto);

        Task<PagedResultDto<GlobalConfigurationDto>> GetAsync(GlobalGetByPagedInputDto globalGetPaged);

        Task DeleteAsync(GlobalGetByAppIdInputDto globalGetByAppId);
    }
}

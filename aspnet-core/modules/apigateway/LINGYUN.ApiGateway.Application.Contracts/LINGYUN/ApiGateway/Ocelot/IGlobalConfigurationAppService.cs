using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IGlobalConfigurationAppService : IApplicationService
    {
        Task<GlobalConfigurationDto> GetAsync(GlobalGetByAppIdInputDto input);

        Task<GlobalConfigurationDto> CreateAsync(GlobalCreateDto input);

        Task<GlobalConfigurationDto> UpdateAsync(GlobalUpdateDto input);

        Task<PagedResultDto<GlobalConfigurationDto>> GetAsync(GlobalGetByPagedInputDto input);

        Task DeleteAsync(GlobalGetByAppIdInputDto input);
    }
}

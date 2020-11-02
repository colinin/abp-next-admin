using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IReRouteAppService : IApplicationService
    {
        Task<ListResultDto<ReRouteDto>> GetListByAppIdAsync(ReRouteGetByAppIdInputDto input);

        Task<PagedResultDto<ReRouteDto>> GetListAsync(ReRouteGetByPagedInputDto input);

        Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto input);

        Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto input);

        Task<ReRouteDto> CreateAsync(ReRouteCreateDto input);

        Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto input);

        Task DeleteAsync(ReRouteGetByIdInputDto input);

        Task RemoveAsync(ReRouteGetByAppIdInputDto input);
    }
}

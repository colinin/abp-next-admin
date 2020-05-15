using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IReRouteAppService : IApplicationService
    {
        Task<ListResultDto<ReRouteDto>> GetAsync(ReRouteGetByAppIdInputDto routeGetByAppId);

        Task<PagedResultDto<ReRouteDto>> GetPagedListAsync(ReRouteGetByPagedInputDto routeGetByPaged);

        Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto routeGetByName);

        Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto routeGetById);

        Task<ReRouteDto> CreateAsync(ReRouteCreateDto routeCreateDto);

        Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto routeUpdateDto);

        Task DeleteAsync(ReRouteGetByIdInputDto routeGetById);

        Task RemoveAsync(ReRouteGetByAppIdInputDto routeGetByAppId);
    }
}

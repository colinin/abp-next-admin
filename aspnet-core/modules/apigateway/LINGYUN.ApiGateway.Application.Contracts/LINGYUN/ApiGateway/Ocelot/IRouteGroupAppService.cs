using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IRouteGroupAppService
    {
        Task<ListResultDto<RouteGroupAppIdsDto>> GetActivedAsync();

        Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto routerGetByAppId);

        Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto routerGetByPagedInput);

        Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto routerCreateDto);

        Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto routerUpdateDto);

        Task DeleteAsync(RouteGroupGetByAppIdInputDto routerGetByAppId);
    }
}

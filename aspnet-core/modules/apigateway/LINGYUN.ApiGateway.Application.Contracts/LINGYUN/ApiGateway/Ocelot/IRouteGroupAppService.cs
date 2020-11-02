using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IRouteGroupAppService
    {
        Task<ListResultDto<RouteGroupAppIdsDto>> GetActivedAsync();

        Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto input);

        Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto input);

        Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto input);

        Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto input);

        Task DeleteAsync(RouteGroupGetByAppIdInputDto input);
    }
}

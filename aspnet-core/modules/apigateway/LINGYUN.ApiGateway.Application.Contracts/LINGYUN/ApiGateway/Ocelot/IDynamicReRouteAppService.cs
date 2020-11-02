using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IDynamicReRouteAppService : IApplicationService
    {
        Task<ListResultDto<DynamicReRouteDto>> GetAsync(DynamicRouteGetByAppIdInputDto input);
    }
}

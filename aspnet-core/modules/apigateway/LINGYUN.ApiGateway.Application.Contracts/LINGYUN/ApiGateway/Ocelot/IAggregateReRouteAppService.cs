using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IAggregateReRouteAppService : IApplicationService
    {
        Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto input);

        Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto input);

        Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto input);

        Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto input);

        Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto input);

        Task DeleteAsync(AggregateRouteGetByRouteIdInputDto input);

        Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto input);

        Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto input);
    }
}

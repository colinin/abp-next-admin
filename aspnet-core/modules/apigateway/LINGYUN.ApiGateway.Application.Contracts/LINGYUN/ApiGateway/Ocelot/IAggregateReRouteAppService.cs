using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IAggregateReRouteAppService : IApplicationService
    {
        Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId);

        Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto aggregateRouteGetByAppId);

        Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto aggregateRouteGetByPaged);

        Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto aggregateReRouteCreate);

        Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto aggregateReRouteUpdate);

        Task DeleteAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId);

        Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto aggregateReRouteConfigCreate);

        Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto aggregateReRouteConfigGetByKey);
    }
}

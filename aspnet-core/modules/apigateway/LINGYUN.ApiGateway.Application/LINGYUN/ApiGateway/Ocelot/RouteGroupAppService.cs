using LINGYUN.ApiGateway.Data.Filter;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Authorize(ApiGatewayPermissions.RouteGroup.Default)]
    public class RouteGroupAppService : ApiGatewayApplicationServiceBase, IRouteGroupAppService
    {
        private IDataFilter _dataFilter;
        protected IDataFilter DataFilter => LazyGetRequiredService(ref _dataFilter);
        protected IRouteGroupRepository RouterRepository { get; }

        public RouteGroupAppService(
            IRouteGroupRepository routerRepository)
        {
            RouterRepository = routerRepository;
        }

        [Authorize(ApiGatewayPermissions.RouteGroup.Create)]
        public virtual async Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto routerCreateDto)
        {
            var router = new RouteGroup(routerCreateDto.AppId, routerCreateDto.AppName, routerCreateDto.AppIpAddress);
            router.Name = routerCreateDto.Name;
            router.IsActive = routerCreateDto.IsActive;
            router.Description = routerCreateDto.Description;

            router = await RouterRepository.InsertAsync(router, true);

            return ObjectMapper.Map<RouteGroup, RouteGroupDto>(router);
        }

        public virtual async Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto routerGetByAppId)
        {
            using (DataFilter.Disable<IActivation>())
            {
                var router = await RouterRepository.GetByAppIdAsync(routerGetByAppId.AppId);

                return ObjectMapper.Map<RouteGroup, RouteGroupDto>(router);
            }
        }

        public virtual async Task<ListResultDto<RouteGroupAppIdsDto>> GetActivedAsync()
        {
            var appIdsDto = new List<RouteGroupAppIdsDto>();
            var appKeys = await RouterRepository.GetActivedAppsAsync();

            foreach(var app in appKeys)
            {
                appIdsDto.Add(new RouteGroupAppIdsDto(app.AppId, app.AppName));
            }

            return new ListResultDto<RouteGroupAppIdsDto>(appIdsDto);
        }

        public virtual async Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto routerGetByPagedInput)
        {
            using (DataFilter.Disable<IActivation>())
            {
                var (Routers, TotalCount) = await RouterRepository.GetPagedListAsync(routerGetByPagedInput.Filter,
                routerGetByPagedInput.Sorting, routerGetByPagedInput.SkipCount, routerGetByPagedInput.MaxResultCount);
                var routers = ObjectMapper.Map<List<RouteGroup>, List<RouteGroupDto>>(Routers);

                return new PagedResultDto<RouteGroupDto>(TotalCount, routers);
            }
        }

        [Authorize(ApiGatewayPermissions.RouteGroup.Update)]
        public virtual async Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto routerUpdateDto)
        {
            var router = await RouterRepository.GetByAppIdAsync(routerUpdateDto.AppId);
            router.Name = routerUpdateDto.Name;
            router.IsActive = routerUpdateDto.IsActive;
            router.Description = routerUpdateDto.Description;

            return ObjectMapper.Map<RouteGroup, RouteGroupDto>(router);
        }

        [Authorize(ApiGatewayPermissions.RouteGroup.Delete)]
        public virtual async Task DeleteAsync(RouteGroupGetByAppIdInputDto routerGetByAppId)
        {
            var router = await RouterRepository.GetByAppIdAsync(routerGetByAppId.AppId);
            await RouterRepository.DeleteAsync(router);
        }
    }
}

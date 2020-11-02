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
        public virtual async Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto input)
        {
            var router = new RouteGroup(input.AppId, input.AppName, input.AppIpAddress);
            router.Name = input.Name;
            router.IsActive = input.IsActive;
            router.Description = input.Description;

            router = await RouterRepository.InsertAsync(router, true);

            return ObjectMapper.Map<RouteGroup, RouteGroupDto>(router);
        }

        public virtual async Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto input)
        {
            using (DataFilter.Disable<IActivation>())
            {
                var router = await RouterRepository.GetByAppIdAsync(input.AppId);

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

        public virtual async Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto input)
        {
            using (DataFilter.Disable<IActivation>())
            {
                var (Routers, TotalCount) = await RouterRepository.GetPagedListAsync(input.Filter,
                input.Sorting, input.SkipCount, input.MaxResultCount);
                var routers = ObjectMapper.Map<List<RouteGroup>, List<RouteGroupDto>>(Routers);

                return new PagedResultDto<RouteGroupDto>(TotalCount, routers);
            }
        }

        [Authorize(ApiGatewayPermissions.RouteGroup.Update)]
        public virtual async Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto input)
        {
            var router = await RouterRepository.GetByAppIdAsync(input.AppId);
            router.Name = input.Name;
            router.IsActive = input.IsActive;
            router.Description = input.Description;
            router.SwitchApp(input.AppName, input.AppIpAddress);

            await RouterRepository.UpdateAsync(router);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<RouteGroup, RouteGroupDto>(router);
        }

        [Authorize(ApiGatewayPermissions.RouteGroup.Delete)]
        public virtual async Task DeleteAsync(RouteGroupGetByAppIdInputDto input)
        {
            var router = await RouterRepository.GetByAppIdAsync(input.AppId);
            await RouterRepository.DeleteAsync(router);
        }
    }
}

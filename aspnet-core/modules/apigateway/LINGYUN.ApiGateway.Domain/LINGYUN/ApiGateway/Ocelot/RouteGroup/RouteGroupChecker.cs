using LINGYUN.ApiGateway.Data.Filter;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupChecker : IRouteGroupChecker, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IRouteGroupRepository _routeGroupRepository;
        public RouteGroupChecker(
            IDataFilter dataFilter,
            IRouteGroupRepository routeGroupRepository)
        {
            _dataFilter = dataFilter;
            _routeGroupRepository = routeGroupRepository;
        }
        public virtual async Task CheckActiveAsync(string appId)
        {
            using (_dataFilter.Disable<IActivation>())
            {
                var routeGroup = await _routeGroupRepository.GetByAppIdAsync(appId);
                if(!routeGroup.IsActive)
                {
                    throw new UserFriendlyException($"查询的路由应用:{routeGroup.AppName} 未启用!");
                }
            }
        }
    }
}

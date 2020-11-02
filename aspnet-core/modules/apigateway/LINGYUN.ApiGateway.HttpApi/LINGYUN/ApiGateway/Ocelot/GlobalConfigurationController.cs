using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace LINGYUN.ApiGateway.Ocelot
{
    [RemoteService(Name = ApiGatewayConsts.RemoteServiceName)]
    [Area("ApiGateway")]
    [Route("api/ApiGateway/Globals")]
    public class GlobalConfigurationController : ApiGatewayControllerBase, IGlobalConfigurationAppService
    {
        protected IGlobalConfigurationAppService GlobalConfigurationAppService { get; }
        protected IPermissionValueProviderManager PermissionValueProviderManager { get; }
        public GlobalConfigurationController(
            IGlobalConfigurationAppService globalConfigurationAppService,
            IPermissionValueProviderManager permissionValueProviderManager)
        {
            GlobalConfigurationAppService = globalConfigurationAppService;
            PermissionValueProviderManager = permissionValueProviderManager;
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public async Task<GlobalConfigurationDto> GetAsync(GlobalGetByAppIdInputDto input)
        {
            return await GlobalConfigurationAppService.GetAsync(input);
        }

        [HttpPost]
        public async Task<GlobalConfigurationDto> CreateAsync(GlobalCreateDto input)
        {
            return await GlobalConfigurationAppService.CreateAsync(input);
        }

        [HttpPut]
        public async Task<GlobalConfigurationDto> UpdateAsync(GlobalUpdateDto input)
        {
            return await GlobalConfigurationAppService.UpdateAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GlobalGetByAppIdInputDto input)
        {
            await GlobalConfigurationAppService.DeleteAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<GlobalConfigurationDto>> GetAsync(GlobalGetByPagedInputDto input)
        {
            var user = CurrentUser;
            return await GlobalConfigurationAppService.GetAsync(input);
        }
    }
}

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
        public async Task<GlobalConfigurationDto> GetAsync(GlobalGetByAppIdInputDto globalGetByAppId)
        {
            return await GlobalConfigurationAppService.GetAsync(globalGetByAppId);
        }

        [HttpPost]
        public async Task<GlobalConfigurationDto> CreateAsync(GlobalCreateDto globalCreateDto)
        {
            return await GlobalConfigurationAppService.CreateAsync(globalCreateDto);
        }

        [HttpPut]
        public async Task<GlobalConfigurationDto> UpdateAsync(GlobalUpdateDto globalUpdateDto)
        {
            return await GlobalConfigurationAppService.UpdateAsync(globalUpdateDto);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GlobalGetByAppIdInputDto globalGetByAppId)
        {
            await GlobalConfigurationAppService.DeleteAsync(globalGetByAppId);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<GlobalConfigurationDto>> GetAsync(GlobalGetByPagedInputDto globalGetPaged)
        {
            var user = CurrentUser;
            return await GlobalConfigurationAppService.GetAsync(globalGetPaged);
        }
    }
}

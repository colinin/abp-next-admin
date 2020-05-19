using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("IdentityServer")]
    [Route("api/IdentityServer/ApiResources")]
    public class ApiResourceController : AbpController, IApiResourceAppService
    {
        protected IApiResourceAppService ApiResourceAppService { get; }
        public ApiResourceController(
            IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
        }

        [HttpGet]
        [Route("{Id}")]
        public virtual async Task<ApiResourceDto> GetAsync(ApiResourceGetByIdInputDto apiResourceGetById)
        {
            return await ApiResourceAppService.GetAsync(apiResourceGetById);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ApiResourceDto>> GetAsync(ApiResourceGetByPagedInputDto apiResourceGetByPaged)
        {
            return await ApiResourceAppService.GetAsync(apiResourceGetByPaged);
        }

        [HttpPost]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateDto apiResourceCreate)
        {
            return await ApiResourceAppService.CreateAsync(apiResourceCreate);
        }

        [HttpPut]
        public virtual async Task<ApiResourceDto> UpdateAsync(ApiResourceUpdateDto apiResourceUpdate)
        {
            return await ApiResourceAppService.UpdateAsync(apiResourceUpdate);
        }

        [HttpDelete]
        [Route("{Id}")]
        public virtual async Task DeleteAsync(ApiResourceGetByIdInputDto apiResourceGetById)
        {
            await ApiResourceAppService.DeleteAsync(apiResourceGetById);
        }

        [HttpPost]
        [Route("Secrets")]
        public virtual async Task<ApiSecretDto> AddSecretAsync(ApiSecretCreateDto apiSecretCreate)
        {
            return await ApiResourceAppService.AddSecretAsync(apiSecretCreate);
        }

        [HttpDelete]
        [Route("Secrets")]
        public virtual async Task DeleteSecretAsync(ApiSecretGetByTypeInputDto apiSecretGetByType)
        {
            await ApiResourceAppService.DeleteSecretAsync(apiSecretGetByType);
        }

        [HttpPost]
        [Route("Scopes")]
        public virtual async Task<ApiScopeDto> AddScopeAsync(ApiScopeCreateDto apiScopeCreate)
        {
            return await ApiResourceAppService.AddScopeAsync(apiScopeCreate);
        }

        [HttpDelete]
        [Route("Scopes")]
        public virtual async Task DeleteScopeAsync(ApiScopeGetByNameInputDto apiScopeGetByName)
        {
            await ApiResourceAppService.DeleteScopeAsync(apiScopeGetByName);
        }
    }
}

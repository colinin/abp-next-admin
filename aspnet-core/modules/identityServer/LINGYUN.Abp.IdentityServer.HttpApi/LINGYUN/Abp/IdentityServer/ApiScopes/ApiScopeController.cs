using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("identity-server")]
    [Route("api/identity-server/api-scopes")]
    public class ApiScopeController : AbpController, IApiScopeAppService
    {
        protected IApiScopeAppService ApiScopeAppService { get; }

        public ApiScopeController(
            IApiScopeAppService apiScopeAppService)
        {
            ApiScopeAppService = apiScopeAppService;
        }

        [HttpPost]
        public virtual async Task<ApiScopeDto> CreateAsync(ApiScopeCreateDto input)
        {
            return await ApiScopeAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await ApiScopeAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<ApiScopeDto> GetAsync(Guid id)
        {
            return await ApiScopeAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ApiScopeDto>> GetListAsync(GetApiScopeInput input)
        {
            return await ApiScopeAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeUpdateDto input)
        {
            return await ApiScopeAppService.UpdateAsync(id, input);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("identity-server")]
    [Route("api/identity-server/api-resources")]
    public class ApiResourceController : AbpController, IApiResourceAppService
    {
        protected IApiResourceAppService ApiResourceAppService { get; }
        public ApiResourceController(
            IApiResourceAppService apiResourceAppService)
        {
            ApiResourceAppService = apiResourceAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<ApiResourceDto> GetAsync(Guid id)
        {
            return await ApiResourceAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ApiResourceDto>> GetListAsync(ApiResourceGetByPagedInputDto input)
        {
            return await ApiResourceAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateDto input)
        {
            return await ApiResourceAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceUpdateDto input)
        {
            return await ApiResourceAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await ApiResourceAppService.DeleteAsync(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.LocalizationManagement
{
    [RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
    [Area("localization")]
    [Route("api/localization/resources")]
    public class ResourceController : AbpController, IResourceAppService
    {
        private readonly IResourceAppService _service;

        public ResourceController(IResourceAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public virtual async Task<ResourceDto> CreateAsync(CreateOrUpdateResourceInput input)
        {
            return await _service.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<ListResultDto<ResourceDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<ResourceDto> GetAsync(Guid id)
        {
            return await _service.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ResourceDto>> GetListAsync(GetResourcesInput input)
        {
            return await _service.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<ResourceDto> UpdateAsync(Guid id, CreateOrUpdateResourceInput input)
        {
            return await _service.UpdateAsync(id, input);
        }
    }
}

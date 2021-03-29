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
    [Route("api/localization/languages")]
    public class LanguageController : AbpController, ILanguageAppService
    {
        private readonly ILanguageAppService _service;

        public LanguageController(ILanguageAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public virtual async Task<LanguageDto> CreateAsync(CreateOrUpdateLanguageInput input)
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
        public virtual async Task<ListResultDto<LanguageDto>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<LanguageDto> GetAsync(Guid id)
        {
            return await _service.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<LanguageDto>> GetListAsync(GetLanguagesInput input)
        {
            return await _service.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<LanguageDto> UpdateAsync(Guid id, CreateOrUpdateLanguageInput input)
        {
            return await _service.UpdateAsync(id, input);
        }
    }
}

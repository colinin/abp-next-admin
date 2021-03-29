using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.LocalizationManagement
{
    [RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
    [Area("localization")]
    [Route("api/localization/texts")]
    public class TextController : AbpController, ITextAppService
    {
        private readonly ITextAppService _service;

        public TextController(ITextAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public virtual async Task<TextDto> CreateAsync(CreateTextInput input)
        {
            return await _service.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<TextDto> GetAsync(int id)
        {
            return await _service.GetAsync(id);
        }

        [HttpGet]
        [Route("by-culture-key")]
        public virtual async Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input)
        {
            return await _service.GetByCultureKeyAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<TextDifferenceDto>> GetListAsync(GetTextsInput input)
        {
            return await _service.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<TextDto> UpdateAsync(int id, UpdateTextInput input)
        {
            return await _service.UpdateAsync(id, input);
        }
    }
}

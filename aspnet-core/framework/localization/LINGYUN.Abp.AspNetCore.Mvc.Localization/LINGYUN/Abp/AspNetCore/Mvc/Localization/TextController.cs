using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Area("abp")]
    [RemoteService(Name = "abp")]
    [Route("api/abp/localization/texts")]
    public class TextController : AbpControllerBase, ITextAppService
    {
        private readonly ITextAppService _service;

        public TextController(ITextAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("by-culture-key")]
        public virtual Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input)
        {
            return _service.GetByCultureKeyAsync(input);
        }

        [HttpGet]
        public virtual Task<ListResultDto<TextDifferenceDto>> GetListAsync(GetTextsInput input)
        {
            return _service.GetListAsync(input);
        }
    }
}

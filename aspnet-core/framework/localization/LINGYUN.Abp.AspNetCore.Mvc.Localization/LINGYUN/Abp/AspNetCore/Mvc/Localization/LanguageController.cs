using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Area("abp")]
    [RemoteService(Name = "abp")]
    [Route("api/abp/localization/languages")]
    public class LanguageController : AbpControllerBase, ILanguageAppService
    {
        private readonly ILanguageAppService _service;

        public LanguageController(ILanguageAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual Task<ListResultDto<LanguageDto>> GetListAsync(GetLanguageWithFilterDto input)
        {
            return _service.GetListAsync(input);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.LocalizationManagement
{
    [RemoteService(Name = LocalizationRemoteServiceConsts.RemoteServiceName)]
    [Area("localization")]
    [Route("api/localization/texts")]
    public class TextController : AbpControllerBase, ITextAppService
    {
        private readonly ITextAppService _service;

        public TextController(ITextAppService service)
        {
            _service = service;
        }

        [HttpPut]
        public virtual Task SetTextAsync(SetTextInput input)
        {
            return _service.SetTextAsync(input);
        }

        [HttpDelete]
        [Route("restore-to-default")]
        public virtual Task RestoreToDefaultAsync(RestoreDefaultTextInput input)
        {
            return _service.RestoreToDefaultAsync(input);
        }
    }
}

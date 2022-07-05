using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Area("abp")]
    [RemoteService(Name = "abp")]
    [Route("api/abp/localization/resources")]
    public class ResourceController : AbpController, IResourceAppService
    {
        private readonly IResourceAppService _service;

        public ResourceController(IResourceAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual Task<ListResultDto<ResourceDto>> GetListAsync()
        {
            return _service.GetListAsync();
        }
    }
}

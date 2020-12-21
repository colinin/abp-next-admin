using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Layouts
{
    [RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
    [Area("platform")]
    [Route("api/platform/layouts")]
    public class LayoutController : AbpController, ILayoutAppService
    {
        protected ILayoutAppService LayoutAppService { get; }

        public LayoutController(
            ILayoutAppService layoutAppService)
        {
            LayoutAppService = layoutAppService;
        }

        [HttpPost]
        public virtual async Task<LayoutDto> CreateAsync(LayoutCreateDto input)
        {
            return await LayoutAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await LayoutAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<LayoutDto> GetAsync(Guid id)
        {
            return await LayoutAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<ListResultDto<LayoutDto>> GetAllListAsync()
        {
            return await LayoutAppService.GetAllListAsync();
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<LayoutDto>> GetListAsync(GetLayoutListInput input)
        {
            return await LayoutAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<LayoutDto> UpdateAsync(Guid id, LayoutUpdateDto input)
        {
            return await LayoutAppService.UpdateAsync(id, input);
        }
    }
}

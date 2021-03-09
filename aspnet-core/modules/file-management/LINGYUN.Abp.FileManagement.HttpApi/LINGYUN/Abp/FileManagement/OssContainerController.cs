using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.FileManagement
{
    [RemoteService(Name = "AbpFileManagement")]
    [Area("file-management")]
    [Route("api/file-management/containes")]
    public class OssContainerController : AbpController, IOssContainerAppService
    {
        protected IOssContainerAppService OssContainerAppService { get; }

        public OssContainerController(
            IOssContainerAppService ossContainerAppService)
        {
            OssContainerAppService = ossContainerAppService;
        }

        [HttpPost]
        [Route("{name}")]
        public virtual async Task<OssContainerDto> CreateAsync(string name)
        {
            return await OssContainerAppService.CreateAsync(name);
        }

        [HttpDelete]
        [Route("{name}")]
        public virtual async Task DeleteAsync(string name)
        {
            await OssContainerAppService.DeleteAsync(name);
        }

        [HttpGet]
        [Route("{name}")]
        public virtual async Task<OssContainerDto> GetAsync(string name)
        {
            return await OssContainerAppService.GetAsync(name);
        }

        [HttpGet]
        public virtual async Task<OssContainersResultDto> GetListAsync(GetOssContainersInput input)
        {
            return await OssContainerAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("objects")]
        public virtual async Task<OssObjectsResultDto> GetObjectListAsync(GetOssObjectsInput input)
        {
            return await OssContainerAppService.GetObjectListAsync(input);
        }
    }
}

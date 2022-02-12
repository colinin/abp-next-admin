using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Auditing.SecurityLogs
{
    [RemoteService(Name = AuditingRemoteServiceConsts.RemoteServiceName)]
    [Area("auditing")]
    [ControllerName("security-log")]
    [Route("api/auditing/security-log")]
    public class SecurityLogController : AbpController, ISecurityLogAppService
    {
        protected ISecurityLogAppService SecurityLogAppService { get; }

        public SecurityLogController(ISecurityLogAppService securityLogAppService)
        {
            SecurityLogAppService = securityLogAppService;
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await SecurityLogAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<SecurityLogDto> GetAsync(Guid id)
        {
            return await SecurityLogAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetByPagedDto input)
        {
            return await SecurityLogAppService.GetListAsync(input);
        }
    }
}

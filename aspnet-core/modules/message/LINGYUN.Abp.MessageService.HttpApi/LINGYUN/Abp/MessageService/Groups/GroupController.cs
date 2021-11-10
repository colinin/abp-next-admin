using LINGYUN.Abp.IM.Groups;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Groups
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/im/groups")]
    public class GroupController : AbpController, IGroupAppService
    {
        private readonly IGroupAppService _service;

        public GroupController(IGroupAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{groupId}")]
        public virtual async Task<Group> GetAsync(string groupId)
        {
            return await _service.GetAsync(groupId);
        }

        [HttpGet]
        [Route("search")]
        public virtual async Task<PagedResultDto<Group>> SearchAsync(GroupSearchInput input)
        {
            return await _service.SearchAsync(input);
        }
    }
}

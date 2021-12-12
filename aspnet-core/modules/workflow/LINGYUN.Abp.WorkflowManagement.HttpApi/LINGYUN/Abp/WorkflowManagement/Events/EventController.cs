using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.Events
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("WorkflowManagement")]
    [Route("api/workflow-management/events")]
    public class EventController : AbpControllerBase, IEventAppService
    {
        private readonly IEventAppService _service;

        public EventController(IEventAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("{EventName}/{EventKey}")]
        public virtual async Task PublishAsync(EventPublishInput input)
        {
            await _service.PublishAsync(input);
        }
    }
}

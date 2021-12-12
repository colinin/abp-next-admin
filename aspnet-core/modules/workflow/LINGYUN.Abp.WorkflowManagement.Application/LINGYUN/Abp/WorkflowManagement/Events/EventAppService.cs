using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowManagement.Events
{
    [Authorize]
    public class EventAppService : WorkflowManagementAppServiceBase, IEventAppService
    {
        private readonly IWorkflowController _controller;

        public EventAppService(IWorkflowController controller)
        {
            _controller = controller;
        }

        public virtual async Task PublishAsync(EventPublishInput input)
        {
            await _controller.PublishEvent(input.EventName, input.EventKey, input.EventData, input.EffectiveDate);
        }
    }
}

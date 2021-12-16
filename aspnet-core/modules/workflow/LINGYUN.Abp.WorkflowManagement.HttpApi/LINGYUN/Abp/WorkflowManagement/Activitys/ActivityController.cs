using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("WorkflowManagement")]
    [Route("api/workflow-management/activitys")]
    public class ActivityController : AbpControllerBase, IActivityAppService
    {
        private readonly IActivityAppService _service;

        public ActivityController(IActivityAppService service)
        {
            _service = service;
        }

        [HttpPost("fail/{token}")]
        public virtual async Task FailureAsync(ActivityFailureInput input)
        {
            await _service.FailureAsync(input);
        }

        [HttpGet("{ActivityName}")]
        public virtual async Task<PendingActivityDto> GetAsync(GetPendingActivityInput input)
        {
            return await _service.GetAsync(input);
        }

        [HttpDelete("{Token}")]
        public virtual async Task DeleteAsync(ActivityReleaseInput input)
        {
            await _service.DeleteAsync(input);
        }

        [HttpPost("success/{Token}")]
        public virtual async Task SuccessAsync(ActivitySuccessInput input)
        {
            await _service.SuccessAsync(input);
        }
    }
}

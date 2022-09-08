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
        public async virtual Task FailureAsync(ActivityFailureInput input)
        {
            await _service.FailureAsync(input);
        }

        [HttpGet("{ActivityName}")]
        public async virtual Task<PendingActivityDto> GetAsync(GetPendingActivityInput input)
        {
            return await _service.GetAsync(input);
        }

        [HttpDelete("{Token}")]
        public async virtual Task DeleteAsync(ActivityReleaseInput input)
        {
            await _service.DeleteAsync(input);
        }

        [HttpPost("success/{Token}")]
        public async virtual Task SuccessAsync(ActivitySuccessInput input)
        {
            await _service.SuccessAsync(input);
        }
    }
}

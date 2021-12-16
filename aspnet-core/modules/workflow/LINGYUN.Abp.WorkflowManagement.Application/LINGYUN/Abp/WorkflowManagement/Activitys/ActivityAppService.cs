using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    [Authorize]
    public class ActivityAppService : WorkflowManagementAppServiceBase, IActivityAppService
    {
        private readonly IActivityController _controller;

        public ActivityAppService(IActivityController controller)
        {
            _controller = controller;
        }

        public virtual async Task FailureAsync(ActivityFailureInput input)
        {
            await _controller.SubmitActivityFailure(input.Token, input.Result);
        }

        public virtual async Task<PendingActivityDto> GetAsync(GetPendingActivityInput input)
        {
            var activity = await _controller.GetPendingActivity(
                input.ActivityName, input.WorkflowId, input.Timeout);

            return ObjectMapper.Map<PendingActivity, PendingActivityDto>(activity);
        }

        public virtual async Task DeleteAsync(ActivityReleaseInput input)
        {
            await _controller.ReleaseActivityToken(input.Token);
        }

        public virtual async Task SuccessAsync(ActivitySuccessInput input)
        {
            await _controller.SubmitActivitySuccess(input.Token, input.Result);
        }
    }
}

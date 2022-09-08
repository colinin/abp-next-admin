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

        public async virtual Task FailureAsync(ActivityFailureInput input)
        {
            await _controller.SubmitActivityFailure(input.Token, input.Result);
        }

        public async virtual Task<PendingActivityDto> GetAsync(GetPendingActivityInput input)
        {
            var activity = await _controller.GetPendingActivity(
                input.ActivityName, input.WorkflowId, input.Timeout);

            return ObjectMapper.Map<PendingActivity, PendingActivityDto>(activity);
        }

        public async virtual Task DeleteAsync(ActivityReleaseInput input)
        {
            await _controller.ReleaseActivityToken(input.Token);
        }

        public async virtual Task SuccessAsync(ActivitySuccessInput input)
        {
            await _controller.SubmitActivitySuccess(input.Token, input.Result);
        }
    }
}

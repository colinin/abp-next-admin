using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement.Activitys
{
    public interface IActivityAppService : IApplicationService
    {
        Task<PendingActivityDto> GetAsync(GetPendingActivityInput input);

        Task DeleteAsync(ActivityReleaseInput input);

        Task SuccessAsync(ActivitySuccessInput input);

        Task FailureAsync(ActivityFailureInput input);
    }
}

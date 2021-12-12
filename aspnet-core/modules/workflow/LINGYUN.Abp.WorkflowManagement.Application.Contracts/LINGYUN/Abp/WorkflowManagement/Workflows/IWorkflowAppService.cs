using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public interface IWorkflowAppService : IApplicationService
    {
        Task<WorkflowDto> GetAsync(string id);

        Task CreateAsync(WorkflowCreateDto input);

        Task<WorkflowDto> StartAsync(string id, WorkflowStartInput input);

        Task SuspendAsync(string id);

        Task ResumeAsync(string id);

        Task TerminateAsync(string id);
    }
}

using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public interface IWorkflowAppService : IApplicationService
    {
        Task<WorkflowInstanceDto> GetAsync(string id);

        Task<WorkflowInstanceDto> StartAsync(Guid id, WorkflowStartInput input);

        Task SuspendAsync(string id);

        Task ResumeAsync(string id);

        Task TerminateAsync(string id);
    }
}

using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public interface IWorkflowDefinitionAppService
    {
        Task<WorkflowDto> CreateAsync(WorkflowDefinitionCreateDto input);

        Task<WorkflowDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);
    }
}

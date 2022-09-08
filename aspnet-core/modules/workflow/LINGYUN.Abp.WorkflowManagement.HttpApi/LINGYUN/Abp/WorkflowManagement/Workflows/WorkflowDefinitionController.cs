using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("WorkflowManagement")]
    [Route("api/workflow-management/workflows/definition")]
    public class WorkflowDefinitionController : AbpControllerBase, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionAppService _service;

        public WorkflowDefinitionController(
            IWorkflowDefinitionAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public async virtual Task<WorkflowDto> CreateAsync(WorkflowDefinitionCreateDto input)
        {
            return await _service.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async virtual Task DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public async virtual Task<WorkflowDto> GetAsync(Guid id)
        {
            return await _service.GetAsync(id);
        }
    }
}

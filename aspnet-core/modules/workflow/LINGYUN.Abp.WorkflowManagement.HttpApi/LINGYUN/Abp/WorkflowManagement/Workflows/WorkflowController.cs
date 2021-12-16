using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    [RemoteService(Name = WorkflowManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("WorkflowManagement")]
    [Route("api/workflow-management/workflows")]
    public class WorkflowController : AbpControllerBase, IWorkflowAppService
    {
        private readonly IWorkflowAppService _service;

        public WorkflowController(IWorkflowAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<WorkflowInstanceDto> GetAsync(string id)
        {
            return await _service.GetAsync(id);
        }

        [HttpPut]
        [Route("{id}/resume")]
        public virtual async Task ResumeAsync(string id)
        {
            await _service.ResumeAsync(id);
        }

        [HttpPost]
        [Route("{id}/start")]
        public virtual async Task<WorkflowInstanceDto> StartAsync(Guid id, WorkflowStartInput input)
        {
            return await _service.StartAsync(id, input);
        }

        [HttpPut]
        [Route("{id}/suspend")]
        public virtual async Task SuspendAsync(string id)
        {
            await _service.SuspendAsync(id);
        }

        [HttpPut]
        [Route("{id}/terminate")]
        public virtual async Task TerminateAsync(string id)
        {
            await _service.TerminateAsync(id);
        }
    }
}

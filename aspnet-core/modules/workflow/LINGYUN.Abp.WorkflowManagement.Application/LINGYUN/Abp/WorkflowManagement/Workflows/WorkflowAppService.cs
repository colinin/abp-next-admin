using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    [Authorize]
    public class WorkflowAppService : WorkflowManagementAppServiceBase, IWorkflowAppService
    {
        private readonly IWorkflowController _controller;
        private readonly IPersistenceProvider _persistence;

        public WorkflowAppService(
            IWorkflowController controller,
            IPersistenceProvider persistence)
        {
            _controller = controller;
            _persistence = persistence;
        }

        public virtual async Task<WorkflowInstanceDto> GetAsync(string id)
        {
            var workflow = await _persistence.GetWorkflowInstance(id);

            return ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(workflow);
        }

        public virtual async Task ResumeAsync(string id)
        {
            var result = await _controller.ResumeWorkflow(id);
            if (!result)
            {
                throw new BusinessException();
            }
        }

        public virtual async Task<WorkflowInstanceDto> StartAsync(string id, WorkflowStartInput input)
        {
            var workflowData = new Dictionary<string, object>();
            foreach (var data in input.Data)
            {
                if (data.Value is JsonElement element)
                {
                    workflowData.TryAdd(data.Key, JsonConvert.DeserializeObject(element.ToString()));
                }
                else
                {
                    workflowData.TryAdd(data.Key, data.Value);
                }
            }

            var instanceId = await _controller.StartWorkflow(id, workflowData);
            var result = await _persistence.GetWorkflowInstance(instanceId);

            return ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(result);
        }

        public virtual async Task SuspendAsync(string id)
        {
            var result = await _controller.SuspendWorkflow(id);
            if (!result)
            {
                throw new BusinessException();
            }
        }

        public virtual async Task TerminateAsync(string id)
        {
            var result = await _controller.TerminateWorkflow(id);
            if (!result)
            {
                throw new BusinessException();
            }
        }
    }
}

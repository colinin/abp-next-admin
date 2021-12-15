using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    [Authorize]
    public class WorkflowAppService : WorkflowManagementAppServiceBase, IWorkflowAppService
    {
        private readonly IWorkflowController _controller;
        private readonly IPersistenceProvider _persistence;
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowAppService(
            IWorkflowController controller,
            IPersistenceProvider persistence,
            IWorkflowRepository workflowRepository)
        {
            _controller = controller;
            _persistence = persistence;
            _workflowRepository = workflowRepository;
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
                throw new BusinessException(
                        WorkflowManagementErrorCodes.SuspendError,
                        "The workflow cannot be resumed at this time. Please check the log");
            }
        }

        public virtual async Task<WorkflowInstanceDto> StartAsync(Guid id, WorkflowStartInput input)
        {
            var workflowDef = await _workflowRepository.GetAsync(id);

            var workflowData = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var data in input.Data)
            {
                object inputValue = data.Value;
                if (data.Value is JsonElement element)
                {
                    inputValue = JsonConvert.DeserializeObject(element.ToString());
                }
                else
                {
                    var defData = workflowDef.FindData(data.Key);
                    if (defData != null)
                    {
                        if (defData.TryParse(inputValue, out inputValue))
                        {
                            workflowData.TryAdd(data.Key, inputValue);
                            continue;
                        }

                        throw new BusinessException(WorkflowManagementErrorCodes.InvalidInputDataType)
                            .WithData("Property", defData.DisplayName)
                            .WithData("DataType", defData.DataType.ToString());
                    }
                }

                workflowData.TryAdd(data.Key, inputValue);
            }

            if (CurrentTenant.IsAvailable)
            {
                workflowData.TryAdd(nameof(IMultiTenant.TenantId), CurrentTenant.GetId());
            }

            foreach (var data in workflowDef.Datas)
            {
                if (data.IsRequired && !workflowData.ContainsKey(data.Name))
                {
                    throw new BusinessException(WorkflowManagementErrorCodes.MissingRequiredProperty)
                        .WithData("Property", data.DisplayName);
                }
            }

            var instanceId = await _controller.StartWorkflow(workflowDef.Id.ToString(), workflowData);
            var result = await _persistence.GetWorkflowInstance(instanceId);

            return ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(result);
        }

        public virtual async Task SuspendAsync(string id)
        {
            var result = await _controller.SuspendWorkflow(id);
            if (!result)
            {
                throw new BusinessException(
                      WorkflowManagementErrorCodes.SuspendError,
                      "The workflow cannot be suspend at this time. Please check the log");
            }
        }

        public virtual async Task TerminateAsync(string id)
        {
            var result = await _controller.TerminateWorkflow(id);
            if (!result)
            {
                throw new BusinessException(
                    WorkflowManagementErrorCodes.TerminateError,
                    "The workflow cannot be terminated at this time. Please check the log");
            }
        }
    }
}

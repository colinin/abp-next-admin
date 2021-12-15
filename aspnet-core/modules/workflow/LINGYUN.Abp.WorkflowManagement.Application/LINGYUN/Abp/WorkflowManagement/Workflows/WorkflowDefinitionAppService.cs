using LINGYUN.Abp.WorkflowManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    [Authorize(WorkflowManagementPermissions.WorkflowDef.Default)]
    public class WorkflowDefinitionAppService : WorkflowManagementAppServiceBase, IWorkflowDefinitionAppService
    {
        private readonly WorkflowManager _workflowManager;

        private readonly IWorkflowRepository _workflowRepository;
        private readonly IStepNodeRepository _stepNodeRepository;
        private readonly ICompensateNodeRepository _compensateNodeRepository;

        public WorkflowDefinitionAppService(
           WorkflowManager workflowManager,
           IWorkflowRepository workflowRepository,
           IStepNodeRepository stepNodeRepository,
           ICompensateNodeRepository compensateNodeRepository)
        {
            _workflowManager = workflowManager;
            _workflowRepository = workflowRepository;
            _stepNodeRepository = stepNodeRepository;
            _compensateNodeRepository = compensateNodeRepository;
        }

        [Authorize(WorkflowManagementPermissions.WorkflowDef.Create)]
        public virtual async Task<WorkflowDto> CreateAsync(WorkflowDefinitionCreateDto input)
        {
            if (await _workflowRepository.CheckVersionAsync(input.Name, input.Version))
            {
                throw new BusinessException();
            }

            var workflowDef = new Workflow(
                GuidGenerator.Create(),
                input.Name,
                input.DisplayName,
                input.Description,
                input.Version,
                tenantId: CurrentTenant.Id)
            {
                IsEnabled = input.IsEnabled,
            };

            foreach (var data in input.Datas)
            {
                workflowDef.AddData(
                    GuidGenerator,
                    data.Name,
                    data.DisplayName,
                    data.DataType,
                    data.IsRequired,
                    data.IsCaseSensitive);
            }

            var stepDefNodes = new List<StepNode>();
            var stepCompensateDefNodes = new List<CompensateNode>();

            ICollection<CompensateNode> CreateCompensateNodes(StepNode node, ICollection<StepDto> steps)
            {
                var stepNodes = new List<CompensateNode>();
                foreach (var step in steps)
                {
                    var stepNode = new CompensateNode(
                        GuidGenerator.Create(),
                        workflowDef.Id,
                        step.Name,
                        step.StepType,
                        step.CancelCondition,
                        saga: step.Saga,
                        parentId: node.Id,
                        tenantId: CurrentTenant.Id);
                    stepNode.Inputs.AddIfNotContains(step.Inputs);
                    stepNode.Outputs.AddIfNotContains(step.Outputs);
                    stepNode.SelectNextStep.AddIfNotContains(step.SelectNextStep);

                    stepNodes.Add(stepNode);
                }
                return stepNodes;
            }

            foreach (var stepInput in input.Steps)
            {
                var stepNode = new StepNode(
                    GuidGenerator.Create(),
                    workflowDef.Id,
                    stepInput.Name,
                    stepInput.StepType,
                    stepInput.CancelCondition,
                    saga: stepInput.Saga,
                    tenantId: CurrentTenant.Id);
                stepNode.Inputs.AddIfNotContains(stepInput.Inputs);
                stepNode.Outputs.AddIfNotContains(stepInput.Outputs);
                stepNode.SelectNextStep.AddIfNotContains(stepInput.SelectNextStep);

                stepDefNodes.Add(stepNode);
                stepCompensateDefNodes.AddRange(CreateCompensateNodes(stepNode, stepInput.CompensateWith));
            }

            await _workflowRepository.InsertAsync(workflowDef);
            await _stepNodeRepository.InsertManyAsync(stepDefNodes);
            await _compensateNodeRepository.InsertManyAsync(stepCompensateDefNodes);

            _workflowManager.Register(workflowDef, stepDefNodes, stepCompensateDefNodes);

            var workflowDto = ObjectMapper.Map<Workflow, WorkflowDto>(workflowDef);

            workflowDto.Steps.AddRange(
                ObjectMapper.Map<List<StepNode>, List<StepNodeDto>>(stepDefNodes));

            workflowDto.CompensateNodes.AddRange(
                ObjectMapper.Map<List<CompensateNode>, List<StepNodeDto>>(stepCompensateDefNodes));

            return workflowDto;
        }

        [Authorize(WorkflowManagementPermissions.WorkflowDef.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var workflowDef = await _workflowRepository.GetAsync(id);
            var stepDefNodes = await _stepNodeRepository.GetAllChildrenWithWorkflowAsync(workflowDef.Id);
            var compensateDefNodes = await _compensateNodeRepository.GetAllChildrenWithWorkflowAsync(workflowDef.Id);

            await _workflowRepository.DeleteAsync(workflowDef);
            await _stepNodeRepository.DeleteManyAsync(stepDefNodes);
            await _compensateNodeRepository.DeleteManyAsync(compensateDefNodes);

            _workflowManager.UnRegister(workflowDef);
        }

        public virtual async Task<WorkflowDto> GetAsync(Guid id)
        {
            var workflowDef = await _workflowRepository.GetAsync(id);
            var stepDefNodes = await _stepNodeRepository.GetAllChildrenWithWorkflowAsync(workflowDef.Id);
            var compensateDefNodes = await _compensateNodeRepository.GetAllChildrenWithWorkflowAsync(workflowDef.Id);

            var workflowDto = ObjectMapper.Map<Workflow, WorkflowDto>(workflowDef);

            workflowDto.Steps.AddRange(
                ObjectMapper.Map<List<StepNode>, List<StepNodeDto>>(stepDefNodes));

            workflowDto.CompensateNodes.AddRange(
                ObjectMapper.Map<List<CompensateNode>, List<StepNodeDto>>(compensateDefNodes));

            return workflowDto;
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    //[Authorize]
    public class WorkflowAppService : WorkflowManagementAppServiceBase, IWorkflowAppService
    {
        private readonly WorkflowManager _workflowManager;
        private readonly IWorkflowController _controller;
        private readonly IPersistenceProvider _persistence;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IStepNodeRepository _stepNodeRepository;
        private readonly ICompensateNodeRepository _compensateNodeRepository;

        public WorkflowAppService(
            IWorkflowController controller,
            IPersistenceProvider persistence,
            WorkflowManager workflowManager,
            IWorkflowRepository workflowRepository,
            IStepNodeRepository stepNodeRepository,
            ICompensateNodeRepository compensateNodeRepository)
        {
            _controller = controller;
            _persistence = persistence;
            _workflowManager = workflowManager;
            _workflowRepository = workflowRepository;
            _stepNodeRepository = stepNodeRepository;
            _compensateNodeRepository = compensateNodeRepository;
        }

        public virtual async Task<WorkflowDto> GetAsync(string id)
        {
            var workflow = await _persistence.GetWorkflowInstance(id);

            return ObjectMapper.Map<WorkflowInstance, WorkflowDto>(workflow);
        }

        public virtual async Task ResumeAsync(string id)
        {
            var result = await _controller.ResumeWorkflow(id);
            if (!result)
            {
                throw new BusinessException();
            }
        }

        public virtual async Task CreateAsync(WorkflowCreateDto input)
        {
            if (await _workflowRepository.CheckVersionAsync(input.Name, input.Version))
            {
                throw new BusinessException();
            }

            var workflow = new Workflow(
                GuidGenerator.Create(),
                input.Name,
                input.Description,
                input.Description,
                input.Version,
                tenantId: CurrentTenant.Id);

            var stepNodes = new List<StepNode>();
            var stepCompensateNodes = new List<CompensateNode>();

            ICollection<CompensateNode> CreateCompensateNodes(StepNode node, ICollection<StepDto> steps)
            {
                var stepNodes = new List<CompensateNode>();
                foreach (var step in steps)
                {
                    var stepNode = new CompensateNode(
                        GuidGenerator.Create(),
                        workflow.Id,
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
                    workflow.Id,
                    stepInput.Name,
                    stepInput.StepType,
                    stepInput.CancelCondition,
                    saga: stepInput.Saga,
                    tenantId: CurrentTenant.Id);
                stepNode.Inputs.AddIfNotContains(stepInput.Inputs);
                stepNode.Outputs.AddIfNotContains(stepInput.Outputs);
                stepNode.SelectNextStep.AddIfNotContains(stepInput.SelectNextStep);

                stepNodes.Add(stepNode);
                stepCompensateNodes.AddRange(CreateCompensateNodes(stepNode, stepInput.CompensateWith));
            }

            await _workflowRepository.InsertAsync(workflow);
            await _stepNodeRepository.InsertManyAsync(stepNodes);
            await _compensateNodeRepository.InsertManyAsync(stepCompensateNodes);

            _workflowManager.Register(workflow, stepNodes, stepCompensateNodes);
        }

        public virtual async Task<WorkflowDto> StartAsync(string id, WorkflowStartInput input)
        {
            var workflowData = new Dictionary<string, object>();
            foreach (var data in input.Data)
            {
                if (data.Value is JsonElement element)
                {
                    //var dataDic = new Dictionary<string, object>();
                    //var children = element.EnumerateObject();
                    //while (children.MoveNext())
                    //{
                    //    dataDic.TryAdd(children.Current.Name, children.Current.Value.ToString());
                    //}
                    //JsonConvert.DeserializeObject(element.ToString())
                    workflowData.TryAdd(data.Key, JsonConvert.DeserializeObject(element.ToString()));
                }
                else
                {
                    workflowData.TryAdd(data.Key, data.Value);
                }
            }

            var instanceId = await _controller.StartWorkflow(id, workflowData);
            var result = await _persistence.GetWorkflowInstance(instanceId);

            return ObjectMapper.Map<WorkflowInstance, WorkflowDto>(result);
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

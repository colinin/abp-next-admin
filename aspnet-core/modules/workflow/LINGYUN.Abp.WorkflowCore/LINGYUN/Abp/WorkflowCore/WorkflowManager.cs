using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using WorkflowCore.Interface;
using WorkflowCore.Models.DefinitionStorage.v1;
using WorkflowCore.Services.DefinitionStorage;
using WDF = WorkflowCore.Models.WorkflowDefinition;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowManager : IWorkflowManager
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IDefinitionLoader _definitionLoader;

        protected IReadOnlyCollection<WorkflowStepBody> _stepBodys;

        internal void Initlize()
        {

        }

        public WDF BuildWorkflow(WorkflowDefinition definition)
        {
            if (_workflowRegistry.IsRegistered(definition.Id, definition.Version))
            {
                throw new AbpException($"Workflow {definition.Id} has ben registered!");
            }

            var definitionSource = new DefinitionSourceV1()
            {
                Id = definition.Id,
                Version = definition.Version,
                DataType = $"{typeof(Dictionary<string, object>).FullName}, {typeof(Dictionary<string, object>).Assembly.FullName}",
                Description = definition.Title,
            };

            BuildWorkflow(definition.Nodes, definitionSource, _stepBodys, definition.Nodes.First(u => u.Key.ToLower().StartsWith("start")));
            var json = definitionSource.SerializeObject();
            var def = _definitionLoader.LoadDefinition(json, Deserializers.Json);
            return def;
        }

        protected virtual void BuildWorkflow(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, IEnumerable<WorkflowStepBody> stepBodys, WorkflowNode node)
        {
            if (source.Steps.Any(u => u.Id == node.Key))
            {
                return;
            }

            var stepSource = new StepSourceV1
            {
                Id = node.Key,
                Name = node.Key
            };
            WorkflowStepBody stepbody = stepBodys.FirstOrDefault(u => u.Name == node.StepBody.Name);
            if (stepbody == null)
            {
                stepbody = new WorkflowStepBody() { StepBodyType = typeof(NullStepBody) };
            }
            stepSource.StepType = $"{stepbody.StepBodyType.FullName}, {stepbody.StepBodyType.Assembly.FullName}";

            foreach (var input in stepbody.Inputs)
            {
                var value = node.StepBody.Inputs[input.Key].Value;
                if (!(value is IDictionary<string, object> || value is IDictionary<object, object>))
                {
                    value = $"\"{value}\"";
                }
                stepSource.Inputs.AddIfNotContains(new KeyValuePair<string, object>(input.Key, value));
            }
            source.Steps.Add(stepSource);
            BuildBranching(allNodes, source, stepSource, stepBodys, node.NextNodes);
        }
        protected virtual void BuildBranching(IEnumerable<WorkflowNode> allNodes, DefinitionSourceV1 source, StepSourceV1 stepSource, IEnumerable<WorkflowStepBody> stepBodys, IEnumerable<WorkflowConditionNode> nodes)
        {
            foreach (var nextNode in nodes)
            {
                var node = allNodes.First(u => u.Key == nextNode.NodeId);
                stepSource.SelectNextStep[nextNode.NodeId] = "1==1";
                if (nextNode.Conditions.Count() > 0)
                {
                    List<string> exps = new List<string>();
                    foreach (var cond in nextNode.Conditions)
                    {
                        if (cond.Value is string && (!decimal.TryParse(cond.Value.ToString(), out decimal tempValue)))
                        {
                            if (cond.Operator != "==" && cond.Operator != "!=")
                            {
                                throw new AbpException($" if {cond.Field} is type of 'String', the Operator must be \"==\" or \"!=\"");
                            }
                            exps.Add($"data[\"{cond.Field}\"].ToString() {cond.Operator} \"{cond.Value}\"");
                            continue;
                        }
                        exps.Add($"decimal.Parse(data[\"{cond.Field}\"].ToString()) {cond.Operator} {cond.Value}");
                    }
                    stepSource.SelectNextStep[nextNode.NodeId] = string.Join(" && ", exps);
                }

                BuildWorkflow(allNodes, source, stepBodys, node);
            }
        }
    }
}

using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowConditionNode
    {
        public string Label { get; set; }
        public string NodeId { get; set; }
        public IEnumerable<WorkflowConditionCondition> Conditions { get; set; }
        public WorkflowConditionNode()
        {
            Conditions = new List<WorkflowConditionCondition>();
        }
    }
}

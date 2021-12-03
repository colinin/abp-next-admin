using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowNode
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public int[] Position { get; set; }
        public string Type { get; set; }
        public WorkflowStepBody StepBody { get; set; }
        public IEnumerable<string> ParentNodes { get; set; }
        public IEnumerable<WorkflowConditionNode> NextNodes { get; set; }
        public WorkflowNode()
        {
            StepBody = new WorkflowStepBody();
            NextNodes = new List<WorkflowConditionNode>();
        }
    }
}

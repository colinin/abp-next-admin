using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowDefinition
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Group { get; set; }
        public ICollection<WorkflowNode> Nodes { get; set; }
        public ICollection<WorkflowFormData> Inputs { get; set; }
    }
}

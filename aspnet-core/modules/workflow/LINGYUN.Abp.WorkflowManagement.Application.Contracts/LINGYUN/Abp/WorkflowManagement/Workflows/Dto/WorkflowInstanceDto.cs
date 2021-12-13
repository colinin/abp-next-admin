using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class WorkflowInstanceDto
    {
        public string Id { get; set; }
        public object Data { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public ICollection<ExecutionPointerDto> ExecutionPointers { get; set; } = new List<ExecutionPointerDto>();
    }
}

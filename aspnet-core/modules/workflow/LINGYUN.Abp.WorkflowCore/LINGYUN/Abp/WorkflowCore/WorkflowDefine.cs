using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowDefine
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public Type DataType { get; set; }
        public List<WorkflowStepBody> Steps { get; set; }
        public WorkflowDefine()
        {
            Steps = new List<WorkflowStepBody>();
        }
    }
}

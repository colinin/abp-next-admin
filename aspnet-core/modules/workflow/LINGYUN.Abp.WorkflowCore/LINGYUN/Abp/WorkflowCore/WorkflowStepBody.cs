using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowStepBody
    {
        public string Name { get; set; }
        public Type StepBodyType { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, WorkflowParamInput> Inputs { get; set; }
        public WorkflowStepBody()
        {
            Inputs = new Dictionary<string, WorkflowParamInput>();
        }
    }
}

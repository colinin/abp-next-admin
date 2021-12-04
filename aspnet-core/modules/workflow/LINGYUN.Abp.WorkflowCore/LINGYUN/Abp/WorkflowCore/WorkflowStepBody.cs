using System;
using System.Collections.Generic;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore
{
    public class WorkflowStepBody
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Type StepType { get; set; }
        public bool Saga { get; set; }
        public string DisplayName { get; set; }
        public string NextStep { get; set; }
        public string CancelCondition { get; set; }
        public TimeSpan? RetryInterval { get; set; }
        public WorkflowErrorHandling? ErrorBehavior { get; set; }
        public List<WorkflowStepBody> CompensateWith { get; set; }
        public Dictionary<string, object> Inputs { get; set; }
        public Dictionary<string, object> Outputs { get; set; }
        public Dictionary<string, object> SelectNextStep { get; set; }
        
        public WorkflowStepBody()
        {
            CompensateWith = new List<WorkflowStepBody>();
            Inputs = new Dictionary<string, object>();
            Outputs = new Dictionary<string, object>();
            SelectNextStep = new Dictionary<string, object>();
        }
    }
}

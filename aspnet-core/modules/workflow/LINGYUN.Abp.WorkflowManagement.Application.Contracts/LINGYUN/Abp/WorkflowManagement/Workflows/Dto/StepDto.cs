using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class StepDto
    {
        public string Name { get; set; }
        public string StepType { get; set; }
        public string CancelCondition { get; set; }
        public TimeSpan? RetryInterval { get; set; }
        public bool Saga { get; set; }
        public string NextStep { get; set; }
        public List<StepDto> CompensateWith { get; set; } = new List<StepDto>();
        public ExtraPropertyDictionary Inputs { get; set; } = new ExtraPropertyDictionary();
        public ExtraPropertyDictionary Outputs { get; set; } = new ExtraPropertyDictionary();
        public ExtraPropertyDictionary SelectNextStep { get; set; } = new ExtraPropertyDictionary();
    }
}

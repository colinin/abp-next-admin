using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class StepDto
    {
        [Required]
        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxStepTypeLength))]
        public string StepType { get; set; }

        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxCancelConditionLength))]
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

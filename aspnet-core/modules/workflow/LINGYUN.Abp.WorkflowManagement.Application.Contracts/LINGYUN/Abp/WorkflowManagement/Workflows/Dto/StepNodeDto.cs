using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class StepNodeDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string StepType { get; set; }
        public string CancelCondition { get; set; }
        public TimeSpan? RetryInterval { get; set; }
        public bool Saga { get; set; }
        public Guid? ParentId { get; set; }
        public ExtraPropertyDictionary Inputs { get; set; }
        public ExtraPropertyDictionary Outputs { get; set; }
        public ExtraPropertyDictionary SelectNextStep { get; set; }
        public StepNodeDto()
        {
            Inputs = new ExtraPropertyDictionary();
            Outputs = new ExtraPropertyDictionary();
            SelectNextStep = new ExtraPropertyDictionary();
        }
    }
}

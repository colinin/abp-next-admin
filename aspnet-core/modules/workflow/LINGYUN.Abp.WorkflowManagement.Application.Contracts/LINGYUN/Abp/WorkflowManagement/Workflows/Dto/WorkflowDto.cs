using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class WorkflowDto : AuditedEntityDto<Guid>
    {
        public bool IsEnabled { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        public TimeSpan? ErrorRetryInterval { get; set; }

        public List<StepNodeDto> Steps { get; set; } = new List<StepNodeDto>();

        public List<StepNodeDto> CompensateNodes { get; set; } = new List<StepNodeDto>();
    }
}

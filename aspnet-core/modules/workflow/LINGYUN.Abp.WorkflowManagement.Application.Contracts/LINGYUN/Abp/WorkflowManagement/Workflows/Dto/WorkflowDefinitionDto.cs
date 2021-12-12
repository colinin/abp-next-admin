using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class WorkflowDefinitionDto
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }

        public List<StepDto> Steps { get; set; } = new List<StepDto>();
    }
}

using System.Collections.Generic;

namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class WorkflowDefinitionCreateDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

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
        [Required]
        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxNameLength))]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DynamicStringLength(typeof(WorkflowConsts), nameof(WorkflowConsts.MaxDescriptionLength))] 
        public string Description { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int Version { get; set; }

        /// <summary>
        /// 输入数据约束
        /// </summary>
        public List<WorkflowDataDto> Datas { get; set; } = new List<WorkflowDataDto>();

        [Required]
        [MinLength(1)]
        public List<StepDto> Steps { get; set; } = new List<StepDto>();
    }
}

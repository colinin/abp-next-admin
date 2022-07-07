using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobActionCreateDto : BackgroundJobActionCreateOrUpdateDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(BackgroundJobActionConsts), nameof(BackgroundJobActionConsts.MaxNameLength))]
    public string Name { get; set; }
}

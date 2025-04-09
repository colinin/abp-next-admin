using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Portal;

public class EnterpriseCreateDto : EnterpriseCreateOrUpdateDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxNameLength))]
    public string Name { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.EntityChange;

public class RestoreEntityInput
{
    /// <summary>
    /// 实体标识
    /// </summary>
    [Required]
    public string EntityId { get; set; }
    /// <summary>
    /// 还原到某个版本标识
    /// </summary>
    /// <remarks>
    /// 注: 当传递此值时, 将还原到指定版本的NewValue
    /// 否则，还原为最近一次版本的OriginalValue
    /// </remarks>
    public Guid? EntityChangeId { get; set; }
}

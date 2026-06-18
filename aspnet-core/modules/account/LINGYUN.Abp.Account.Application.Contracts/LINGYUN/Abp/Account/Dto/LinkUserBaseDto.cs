using System;

namespace LINGYUN.Abp.Account.Dto;

public abstract class LinkUserBaseDto
{
    /// <summary>
    /// 关联用户Id
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// 关联租户Id
    /// </summary>
    public Guid? TenantId { get; set; }
}

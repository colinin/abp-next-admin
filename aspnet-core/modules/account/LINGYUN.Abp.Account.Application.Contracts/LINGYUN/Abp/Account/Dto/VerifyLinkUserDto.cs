using System;

namespace LINGYUN.Abp.Account.Dto;

public class VerifyLinkUserDto
{
    /// <summary>
    /// 关联用户Id
    /// </summary>
    public Guid? LinkUserId { get; set; }
    /// <summary>
    /// 关联用户名
    /// </summary>
    public string LinkUserName { get; set; }
    /// <summary>
    /// 关联租户Id
    /// </summary>
    public Guid? LinkTenantId { get; set; }
    /// <summary>
    /// 关联租户名
    /// </summary>
    public string LinkTenantName { get; set; }
    /// <summary>
    /// 是否已关联
    /// </summary>
    public bool IsLinked { get; set; }
}

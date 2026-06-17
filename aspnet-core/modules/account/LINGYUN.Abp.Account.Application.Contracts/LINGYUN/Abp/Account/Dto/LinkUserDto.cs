using System;

namespace LINGYUN.Abp.Account.Dto;
/// <summary>
/// 关联用户Dto
/// </summary>
public class LinkUserDto
{
    /// <summary>
    /// 关联用户Id
    /// </summary>
    public Guid LinkUserId { get; set; }
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
    /// 直接关联
    /// </summary>
    public bool DirectlyLinked { get; set; }
}

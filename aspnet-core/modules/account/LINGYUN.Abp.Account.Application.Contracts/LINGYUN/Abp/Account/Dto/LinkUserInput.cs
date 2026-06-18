using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account.Dto;
/// <summary>
/// 关联用户Dto
/// </summary>
public class LinkUserInput : LinkUserBaseDto
{
    /// <summary>
    /// 关联用户Token
    /// </summary>
    [Required]
    public string Token { get; set; }
}

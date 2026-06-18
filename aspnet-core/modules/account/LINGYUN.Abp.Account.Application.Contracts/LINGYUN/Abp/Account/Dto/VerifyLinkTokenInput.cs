using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account.Dto;
/// <summary>
/// 验证关联用户TokenDto
/// </summary>
public class VerifyLinkTokenInput : LinkUserBaseDto
{
    /// <summary>
    /// 关联用户Token
    /// </summary>
    [Required]
    public string Token { get; set; }
}

using System.Collections.Generic;

namespace LINGYUN.Abp.Identity.Session;
/// <summary>
/// 用于会话管理的配置
/// </summary>
public class IdentitySessionSignInOptions
{
    /// <summary>
    /// 用于处理的身份认证方案
    /// </summary>
    public List<string> AuthenticationSchemes { get; set; }
    /// <summary>
    /// 是否启用SignInManager登录会话
    /// 默认: false
    /// </summary>
    public bool SignInSessionEnabled { get; set; }
    /// <summary>
    /// 是否启用SignInManager登出会话
    /// 默认: false
    /// </summary>
    public bool SignOutSessionEnabled { get; set; }

    public IdentitySessionSignInOptions()
    {
        AuthenticationSchemes = new List<string>
        {
            "Identity.Application"
        };
    }
}

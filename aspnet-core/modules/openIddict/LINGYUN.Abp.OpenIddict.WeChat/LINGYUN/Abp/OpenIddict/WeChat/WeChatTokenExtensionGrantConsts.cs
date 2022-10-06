using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.Official;

namespace LINGYUN.Abp.OpenIddict.WeChat;

public static class WeChatTokenExtensionGrantConsts
{
    public static string MiniProgramGrantType => AbpWeChatMiniProgramConsts.GrantType;
    public static string OfficialGrantType => AbpWeChatOfficialConsts.GrantType;
    public static string ProfileKey => AbpWeChatGlobalConsts.ProfileKey;
}

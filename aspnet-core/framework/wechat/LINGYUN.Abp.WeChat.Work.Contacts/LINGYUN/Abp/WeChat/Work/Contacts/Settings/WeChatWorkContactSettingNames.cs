using LINGYUN.Abp.WeChat.Work.Settings;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Settings;

public static class WeChatWorkContactSettingNames
{
    public const string Prefix = WeChatWorkSettingNames.Prefix + ".Contacts";
    /// <summary>
    /// 通讯录应用Srcret
    /// </summary>
    public const string Secret = Prefix + ".Secret";
}

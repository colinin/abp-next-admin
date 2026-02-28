using LINGYUN.Abp.WeChat.Work.Features;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Features;
/// <summary>
/// 企业微信通讯录模块功能
/// </summary>
public static class WeChatWorkContactsFeatureNames
{
    public const string GroupName = WeChatWorkFeatureNames.GroupName + ".Contacts";
    /// <summary>
    /// 启用企业微信通讯录
    /// </summary>
    public const string Enable = GroupName + ".Enable";
}

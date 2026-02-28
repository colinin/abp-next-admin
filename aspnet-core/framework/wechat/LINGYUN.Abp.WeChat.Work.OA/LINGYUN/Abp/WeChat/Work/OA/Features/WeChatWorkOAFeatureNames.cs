using LINGYUN.Abp.WeChat.Work.Features;

namespace LINGYUN.Abp.WeChat.Work.OA.Features;
/// <summary>
/// 企业微信办公模块功能
/// </summary>
public static class WeChatWorkOAFeatureNames
{
    public const string GroupName = WeChatWorkFeatureNames.GroupName + ".OA";
    /// <summary>
    /// 启用企业微信办公
    /// </summary>
    public const string Enable = GroupName + ".Enable";
}

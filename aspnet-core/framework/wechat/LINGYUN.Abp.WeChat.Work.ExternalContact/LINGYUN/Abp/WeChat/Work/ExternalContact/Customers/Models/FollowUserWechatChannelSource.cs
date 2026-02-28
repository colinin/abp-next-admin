using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 视频号添加场景
/// </summary>
[Description("视频号添加场景")]
public enum FollowUserWechatChannelSource
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    None = 0,
    /// <summary>
    /// 视频号主页
    /// </summary>
    [Description("视频号主页")]
    Home = 1,
    /// <summary>
    /// 视频号直播间
    /// </summary>
    [Description("视频号直播间")]
    LiveRoom = 2,
    /// <summary>
    /// 视频号留资服务
    /// </summary>
    [Description("视频号留资服务")]
    RetentionService = 3
}

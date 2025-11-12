using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Models;
/// <summary>
/// 客户来源
/// </summary>
[Description("客户来源")]
public enum FollowUserAddWay
{
    /// <summary>
    /// 未知来源
    /// </summary>
    [Description("未知来源")]
    None = 0,
    /// <summary>
    /// 扫描二维码
    /// </summary>
    [Description("扫描二维码")]
    ScanQrCode = 1,
    /// <summary>
    /// 搜索手机号
    /// </summary>
    [Description("搜索手机号")]
    SearchPhoneNumber = 2,
    /// <summary>
    /// 名片分享
    /// </summary>
    [Description("名片分享")]
    SharedCard = 3,
    /// <summary>
    /// 群聊
    /// </summary>
    [Description("群聊")]
    GroupChat = 4,
    /// <summary>
    /// 手机通讯录
    /// </summary>
    [Description("手机通讯录")]
    PhoneBook = 5,
    /// <summary>
    /// 微信联系人
    /// </summary>
    [Description("微信联系人")]
    WeChatContact = 6,
    /// <summary>
    /// 安装第三方应用时自动添加的客服人员
    /// </summary>
    [Description("安装第三方应用时自动添加的客服人员")]
    InstallThirdPartyApp = 8,
    /// <summary>
    /// 搜索邮箱
    /// </summary>
    [Description("搜索邮箱")]
    SearchEmail = 9,
    /// <summary>
    /// 视频号添加
    /// </summary>
    [Description("视频号添加")]
    WechatChannel = 10,
    /// <summary>
    /// 通过日程参与人添加
    /// </summary>
    [Description("通过日程参与人添加")]
    SchedulePart = 11,
    /// <summary>
    /// 通过会议参与人添加
    /// </summary>
    [Description("通过会议参与人添加")]
    MeetPart = 12,
    /// <summary>
    /// 添加微信好友对应的企业微信
    /// </summary>
    [Description("添加微信好友对应的企业微信")]
    WeChatFriend = 13,
    /// <summary>
    /// 通过智慧硬件专属客服添加
    /// </summary>
    [Description("通过智慧硬件专属客服添加")]
    SmartHardware = 14,
    /// <summary>
    /// 通过上门服务客服添加
    /// </summary>
    [Description("通过上门服务客服添加")]
    DoorService = 15,
    /// <summary>
    /// 通过获客链接添加
    /// </summary>
    [Description("通过获客链接添加")]
    CustomerAcqLink = 16,
    /// <summary>
    /// 通过定制开发添加
    /// </summary>
    [Description("通过定制开发添加")]
    CustomDevelopment = 17,
    /// <summary>
    /// 通过需求回复添加
    /// </summary>
    [Description("通过需求回复添加")]
    DemandResponse = 18,
    /// <summary>
    /// 通过第三方售前客服添加
    /// </summary>
    [Description("通过第三方售前客服添加")]
    ThirdPartyPreSales = 21,
    /// <summary>
    /// 通过可能的商务伙伴添加
    /// </summary>
    [Description("通过可能的商务伙伴添加")]
    PotentialBusPart = 22,
    /// <summary>
    /// 通过接受微信账号收到的好友申请添加
    /// </summary>
    [Description("通过接受微信账号收到的好友申请添加")]
    WeChatFriendRequest = 24,
    /// <summary>
    /// 内部成员共享
    /// </summary>
    [Description("内部成员共享")]
    SharedByInternalMembers = 201,
    /// <summary>
    /// 管理员/负责人分配
    /// </summary>
    [Description("管理员/负责人分配")]
    AllocationByAdmin = 202
}

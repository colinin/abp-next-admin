using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
/// <summary>
/// 接替状态
/// </summary>
[Description("接替状态")]
public enum TransferStatus
{
    /// <summary>
    /// 接替完毕
    /// </summary>
    [Description("接替完毕")]
    Completed = 1,
    /// <summary>
    /// 等待接替
    /// </summary>
    [Description("等待接替")]
    Pending = 2,
    /// <summary>
    /// 客户拒绝
    /// </summary>
    [Description("客户拒绝")]
    CustomerReject = 3,
    /// <summary>
    /// 接替成员客户达到上限
    /// </summary>
    [Description("接替成员客户达到上限")]
    ReachesLimit = 4
}

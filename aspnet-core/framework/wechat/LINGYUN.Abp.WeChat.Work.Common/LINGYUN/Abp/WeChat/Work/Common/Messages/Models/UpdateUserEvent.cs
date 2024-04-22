using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 更新成员事件
/// </summary>
[EventName("update_user")]
public class UpdateUserEvent : UserChangeEvent
{
    /// <summary>
    /// 新的UserID，变更时推送（userid由系统生成时可更改一次）
    /// </summary>
    [XmlElement("NewUserID")]
    public string NewUserId { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<UpdateUserEvent>(this);
    }
}

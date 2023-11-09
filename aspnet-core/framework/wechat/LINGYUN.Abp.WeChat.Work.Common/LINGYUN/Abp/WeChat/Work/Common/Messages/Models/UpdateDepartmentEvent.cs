using LINGYUN.Abp.WeChat.Common.Messages;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 更新部门事件
/// </summary>
[EventName("update_party")]
public class UpdateDepartmentEvent : DepartmentUpdateEvent
{
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<UpdateDepartmentEvent>(this);
    }
}

using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Messages.Models;
/// <summary>
/// 异步任务完成通知
/// </summary>
/// <remarks>
/// 本事件是成员在使用异步任务接口时，用于接收任务执行完毕的结果通知。
/// </remarks>
[EventName("batch_job_result")]
public class BatchJobResultEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 异步任务信息
    /// </summary>
    [XmlElement("BatchJob")]
    public BatchJob BatchJob { get; set; }

    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<BatchJobResultEvent>(this);
    }
}

public class BatchJob
{
    /// <summary>
    /// 异步任务id，最大长度为64字符
    /// </summary>
    [XmlElement("JobId")]
    public string JobId { get; set; }
    /// <summary>
    /// 操作类型，字符串，目前分别有：sync_user(增量更新成员)、 replace_user(全量覆盖成员）、invite_user(邀请成员关注）、replace_party(全量覆盖部门)
    /// </summary>
    [XmlElement("JobType")]
    public string JobType { get; set; }
    /// <summary>
    /// 返回码
    /// </summary>
    [XmlElement("ErrCode")]
    public int ErrCode { get; set; }
    /// <summary>
    /// 对返回码的文本描述内容
    /// </summary>
    [XmlElement("ErrMsg")]
    public string ErrMsg { get; set; }
}
using LINGYUN.Abp.WeChat.Common.Messages;
using System.Xml.Serialization;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
/// <summary>
/// 进入应用事件
/// </summary>
[EventName("batch_job_result")]
public class BatchJobResultEvent : WeChatWorkEventMessage
{
    /// <summary>
    /// 异步任务信息
    /// </summary>
    [XmlElement("BatchJob")]
    public BatchJobResult BatchJob { get; set; }
    public override WeChatMessageEto ToEto()
    {
        return new WeChatWorkEventMessageEto<BatchJobResultEvent>(this);
    }
}

public class BatchJobResult
{
    /// <summary>
    /// 异步任务id，最大长度为64字符
    /// </summary>
    [XmlElement("JobId")]
    public string JobId { get; set; }
    /// <summary>
    /// 操作类型，字符串，
    /// 目前分别有：
    /// sync_user(增量更新成员)、 
    /// replace_user(全量覆盖成员）、
    /// invite_user(邀请成员关注）、
    /// replace_party(全量覆盖部门)
    /// </summary>
    [XmlElement("JobType")]
    public string JobType { get; set; }
    /// <summary>
    /// 返回码
    /// </summary>
    [XmlElement("ErrCode")]
    public int ErrCode { get; set; }
    /// <summary>
    /// 返回码
    /// </summary>
    [XmlElement("ErrMsg")]
    public string ErrMsg { get; set; }
}

using System.ComponentModel;

namespace LINGYUN.Platform.Feedbacks;
public enum FeedbackStatus
{
    /// <summary>
    /// 新增
    /// </summary>
    [Description("FeedbackStatus:Created")]
    Created = 1,
    /// <summary>
    /// 处理中
    /// </summary>
    [Description("FeedbackStatus:InProgress")]
    InProgress = 2,
    /// <summary>
    /// 已关闭
    /// </summary>
    [Description("FeedbackStatus:Closed")]
    Closed = 3,
    /// <summary>
    /// 已解决
    /// </summary>
    [Description("FeedbackStatus:Resolved")]
    Resolved = 4
}

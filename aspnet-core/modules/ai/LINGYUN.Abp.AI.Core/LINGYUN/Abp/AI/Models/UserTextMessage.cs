namespace LINGYUN.Abp.AI.Models;
public class UserTextMessage : UserMessage
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; }
    public UserTextMessage(
        string workspace,
        string content)
        : base(workspace)
    {
        Content = content;
    }

    public override string GetMessagePrompt()
    {
        return Content;
    }
}

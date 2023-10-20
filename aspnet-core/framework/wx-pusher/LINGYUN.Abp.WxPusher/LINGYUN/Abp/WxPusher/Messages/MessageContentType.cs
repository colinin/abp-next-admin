namespace LINGYUN.Abp.WxPusher.Messages;
public enum MessageContentType
{
    /// <summary>
    /// 文字
    /// </summary>
    Text = 1,
    /// <summary>
    /// html(只发送body标签内部的数据即可，不包括body标签
    /// </summary>
    Html = 2,
    /// <summary>
    /// markdown
    /// </summary>
    Markdown = 3
}

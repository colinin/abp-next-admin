namespace LINGYUN.Abp.WeChat.Work.JsSdk.Models;
public class JsApiTicketInfo
{
    /// <summary>
    /// 生成签名所需的 jsapi_ticket，最长为512字节
    /// </summary>
    public string Ticket { get; set; }
    /// <summary>
    /// 凭证的有效时间（秒）
    /// </summary>
    public int ExpiresIn { get; set; }
    public JsApiTicketInfo()
    {

    }

    public JsApiTicketInfo(string ticket, int expiresIn)
    {
        Ticket = ticket;
        ExpiresIn = expiresIn;
    }
}

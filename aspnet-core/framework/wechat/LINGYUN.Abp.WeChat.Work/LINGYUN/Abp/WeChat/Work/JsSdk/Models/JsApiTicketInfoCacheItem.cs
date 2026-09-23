namespace LINGYUN.Abp.WeChat.Work.JsSdk.Models;

public class JsApiTicketInfoCacheItem
{
    public string Ticket { get; set; } = default!;

    public int ExpiresIn { get; set; }

    public JsApiTicketInfoCacheItem()
    {

    }

    public JsApiTicketInfoCacheItem(string ticket, int expiresIn)
    {
        Ticket = ticket;
        ExpiresIn = expiresIn;
    }
}

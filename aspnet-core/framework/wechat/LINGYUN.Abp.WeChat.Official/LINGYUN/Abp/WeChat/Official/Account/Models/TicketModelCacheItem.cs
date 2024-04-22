namespace LINGYUN.Abp.WeChat.Official.Account.Models;
public class TicketModelCacheItem
{
    public string Ticket { get; set; }

    public int ExpireSeconds { get; set; }

    public string Url { get; set; }

    public TicketModelCacheItem()
    {

    }

    public TicketModelCacheItem(string ticket, int expireSeconds, string url)
    {
        Ticket = ticket;
        ExpireSeconds = expireSeconds;
        Url = url;
    }

    public static string CalculateCacheKey(string action, string scene)
    {
        return "a:" + action + ";s:" + scene;
    }
}

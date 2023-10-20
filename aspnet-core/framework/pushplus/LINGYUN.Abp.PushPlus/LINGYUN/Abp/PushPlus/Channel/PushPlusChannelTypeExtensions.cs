namespace LINGYUN.Abp.PushPlus.Channel;
public static class PushPlusChannelTypeExtensions
{
    public static string GetChannelName(this PushPlusChannelType channelType)
    {
        return channelType switch
        {
            PushPlusChannelType.WeChat => "wechat",
            PushPlusChannelType.WeWork => "cp",
            PushPlusChannelType.Webhook => "webhook",
            PushPlusChannelType.Email => "mail",
            PushPlusChannelType.Sms => "sms",
            _ => "wechat",
        };
    }
}

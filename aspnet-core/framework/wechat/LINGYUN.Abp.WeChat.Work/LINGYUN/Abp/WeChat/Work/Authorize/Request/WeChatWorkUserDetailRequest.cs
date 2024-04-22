using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.Authorize.Request;

public class WeChatWorkUserDetailRequest
{
    [NotNull]
    [JsonProperty("user_ticket")]
    [JsonPropertyName("user_ticket")]
    public string UserTicket { get; set; }
    public WeChatWorkUserDetailRequest([NotNull]  string userTicket)
    {
        UserTicket = Check.NotNullOrWhiteSpace(userTicket, nameof(userTicket), 512);
    }

    public virtual string SerializeToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

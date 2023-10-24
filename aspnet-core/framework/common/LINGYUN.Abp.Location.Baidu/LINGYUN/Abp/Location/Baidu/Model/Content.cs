using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class Content
    {
        public string Address { get; set; }

        [JsonProperty("address_detail")]
        [JsonPropertyName("address_detail")]
        public AddressDetail AddressDetail { get; set; } = new AddressDetail();

        public IpPoint Point { get; set; } = new IpPoint();
    }
}

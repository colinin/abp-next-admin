using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class AddressDetail
    {
        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonProperty("city_code")]
        [JsonPropertyName("city_code")]
        public int CityCode { get; set; }

        [JsonProperty("province")]
        [JsonPropertyName("province")]
        public string Province { get; set; }
    }
}

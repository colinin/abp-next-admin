using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 地址部件，address不满足需求时可自行拼接
    /// </summary>
    public class AddressComponent
    {
        /// <summary>
        /// 国家
        /// </summary>
        [JsonProperty("nation")]
        public string Nation { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 区，可能为空字串
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }
        /// <summary>
        /// 街道，可能为空字串
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }
        /// <summary>
        /// 门牌，可能为空字串
        /// </summary>
        [JsonProperty("street_number")]
        public string StreetNumber { get; set; }
    }
}

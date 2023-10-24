using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// IP定位结果
    /// </summary>
    public class TencentIPGeocode
    {
        /// <summary>
        /// 用于定位的IP地址
        /// </summary>
        [JsonProperty("ip")]
        public string IpAddress { get; set; }
        /// <summary>
        /// 定位坐标
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; } = new Location();
        /// <summary>
        /// 定位行政区划信息
        /// </summary>
        [JsonProperty("ad_info")]
        public AddressInfo AddressInfo { get; set; } = new AddressInfo();
    }
}

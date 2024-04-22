using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    public class TencentGeocode
    {
        /// <summary>
        /// 解析到的坐标
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; } = new Location();
        /// <summary>
        /// 解析后的地址部件
        /// </summary>
        [JsonProperty("address_components")]
        public AddressComponent AddressComponent { get; set; } = new AddressComponent();
        /// <summary>
        /// 行政区划信息
        /// </summary>
        [JsonProperty("ad_info")]
        public GeocodeAddressInfo AddressInfo { get; set; } = new GeocodeAddressInfo();
        /// <summary>
        /// 可信度参考：值范围 1 <低可信> - 10 <高可信>
        /// </summary>
        [JsonProperty("reliability")]
        public int Reliability { get; set; }
        /// <summary>
        /// 解析精度级别，分为11个级别，一般>=9即可采用（定位到点，精度较高） 也可根据实际业务需求自行调整
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
    }
    /// <summary>
    /// 行政区划信息
    /// </summary>
    public class GeocodeAddressInfo
    {
        /// <summary>
        /// 行政区划代码
        /// </summary>
        [JsonProperty("adcode")]
        public string AdCode { get; set; }
    }
}

using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 逆地址解析结果
    /// </summary>
    public class TencentReGeocode
    {
        /// <summary>
        /// 地址描述
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// 位置描述
        /// </summary>
        [JsonProperty("formatted_addresses")]
        public FormattedAddress FormattedAddress { get; set; } = new FormattedAddress();
        /// <summary>
        /// 地址部件，address不满足需求时可自行拼接
        /// </summary>
        [JsonProperty("address_component")]
        public AddressComponent AddressComponent { get; set; } = new AddressComponent();
        /// <summary>
        /// 行政区划信息
        /// </summary>
        [JsonProperty("ad_info")]
        public AddressInfo AddressInfo { get; set; } = new AddressInfo();
        /// <summary>
        /// 坐标相对位置参考
        /// </summary>
        [JsonProperty("address_reference")]
        public AddressReference AddressReference { get; set; } = new AddressReference();
        /// <summary>
        /// 查询的周边poi的总数
        /// </summary>
        [JsonProperty("poi_count")]
        public int PoiCount { get; set; }
        /// <summary>
        /// POI数组，对象中每个子项为一个POI对象
        /// </summary>
        [JsonProperty("pois")]
        public Poi[] Pois { get; set; } = new Poi[0];
    }
}

using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 行政区划信息
    /// </summary>
    public class AddressInfo
    {
        [JsonProperty("adcode")]
        public string AdCode { get; set; }
        /// <summary>
        /// 城市代码
        /// </summary>
        [JsonProperty("city_code")]
        public string CityCode { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        [JsonProperty("nation_code")]
        public string NationCode { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        [JsonProperty("nation")]
        public string Nation { get; set; }
        /// <summary>
        /// 省/直辖市
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 市/地级区 及同级行政区划
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 区/县级市 及同级行政区划
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }
        /// <summary>
        /// 行政区划中心点坐标
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; } = new Location();
    }
}

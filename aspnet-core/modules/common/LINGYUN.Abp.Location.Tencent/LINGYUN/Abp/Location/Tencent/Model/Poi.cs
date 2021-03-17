using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// POI对象
    /// </summary>
    public class Poi
    {
        /// <summary>
        /// 地点唯一标识
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 名称/标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// POI分类
        /// </summary>
        [JsonProperty("category")]
        public string CateGory { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; } = new Location();
        /// <summary>
        /// 该POI到逆地址解析传入的坐标的直线距离
        /// </summary>
        [JsonProperty("_distance")]
        public double Distance { get; set; }
        /// <summary>
        /// 行政区划信息
        /// </summary>
        [JsonProperty("ad_info")]
        public AddressInfo AddressInfo { get; set; } = new AddressInfo();
    }
}

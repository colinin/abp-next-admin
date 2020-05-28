using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Amap
{
    /// <summary>
    /// 高德地图的返回参数格式真的奇葩
    /// 弃用了
    /// </summary>
    public class AmapGeocode
    {
        /// <summary>
        /// 结构化地址信息
        /// 省份＋城市＋区县＋城镇＋乡村＋街道＋门牌号码
        /// </summary>
        [JsonProperty("formatted_address")]
        public string Address { get; set; }

        /// <summary>
        /// 国家
        /// 国内地址默认返回中国
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// 地址所在的省份名
        /// 例如：北京市。此处需要注意的是，中国的四大直辖市也算作省级单位。
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }

        /// <summary>
        /// 地址所在的城市名
        /// 例如：北京市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 城市编码
        /// 例如：010
        /// </summary>
        [JsonProperty("citycode")]
        public string CityCode { get; set; }

        /// <summary>
        /// 地址所在的区
        /// 例如：朝阳区
        /// </summary>
        [JsonProperty("district")]
        public string District { get; set; }

        /// <summary>
        /// 坐标点所在乡镇/街道（此街道为社区街道，不是道路信息）
        /// 例如：燕园街道
        /// </summary>
        public string[] TownShip { get; set; } = new string[0];

        /// <summary>
        /// 社区信息列表
        /// </summary>
        public NeighBorHood NeighBorHood { get; set; } = new NeighBorHood();

        /// <summary>
        /// 楼信息列表
        /// </summary>
        public Building Building { get; set; } = new Building();

        /// <summary>
        /// 街道
        /// 例如：阜通东大街
        /// </summary>
        [JsonProperty("street")]
        public string[] Street { get; set; } = new string[0];

        /// <summary>
        /// 门牌
        /// 例如：6号
        /// </summary>
        [JsonProperty("number")]
        public string[] Number { get; set; } = new string[0];

        /// <summary>
        /// 区域编码
        /// 例如：110101
        /// </summary>
        [JsonProperty("adcode")]
        public string Adcode { get; set; }

        /// <summary>
        /// 坐标点
        /// 经度，纬度
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// 匹配级别
        /// 例如：朝阳区
        /// </summary>
        [JsonProperty("level")]
        public string Level { get; set; }
    }
}

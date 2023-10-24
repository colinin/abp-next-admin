using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class BaiduReGeocode
    {
        /// <summary>
        /// 经纬度坐标
        /// </summary>
        [JsonProperty("location")]
        [JsonPropertyName("location")]
        public BaiduLocation Location { get; set; }
        /// <summary>
        /// 结构化地址信息
        /// </summary>
        [JsonProperty("formatted_address")]
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }
        /// <summary>
        /// 坐标所在商圈信息，如 "人民大学,中关村,苏州街"。
        /// 最多返回3个。
        /// </summary>
        public string Business { get; set; }
        /// <summary>
        /// 地址元素列表
        /// </summary>
        public AddressComponent AddressComponent { get; set; }
        /// <summary>
        /// 周边poi数组
        /// </summary>
        public List<BaiduPoi> Pois { get; set; }
        /// <summary>
        /// 周边道路数组
        /// </summary>
        public List<BaiduRoad> Roads { get; set; }
        /// <summary>
        /// 周边区域面数组
        /// </summary>
        public List<PoiRegion> PoiRegions { get; set; }
        /// <summary>
        /// 当前位置结合POI的语义化结果描述
        /// </summary>
        [JsonProperty("sematic_description")]
        [JsonPropertyName("sematic_description")]
        public string SematicDescription { get; set; }
        public BaiduReGeocode()
        {
            AddressComponent = new AddressComponent();
            Pois = new List<BaiduPoi>();
            Roads = new List<BaiduRoad>();
            PoiRegions = new List<PoiRegion>();
        }
    }
}

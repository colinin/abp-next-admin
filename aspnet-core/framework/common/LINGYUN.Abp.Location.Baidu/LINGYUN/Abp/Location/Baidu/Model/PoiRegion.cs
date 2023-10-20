using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    /// <summary>
    /// 区域面
    /// </summary>
    public class PoiRegion
    {
        /// <summary>
        /// 归属区域面名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 归属区域面类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 请求中的坐标与所归属区域面的相对位置关系
        /// </summary>
        [JsonProperty("direction_desc")]
        [JsonPropertyName("direction_desc")]
        public string DirectionDesc { get; set; }
        /// <summary>
        /// poi唯一标识
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 离坐标点距离
        /// </summary>
        public string Distance { get; set; }
    }
}

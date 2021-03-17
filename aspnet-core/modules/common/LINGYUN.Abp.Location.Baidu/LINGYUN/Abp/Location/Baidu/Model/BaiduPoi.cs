using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class BaiduPoi
    {
        /// <summary>
        /// 地址信息
        /// </summary>
        [JsonProperty("addr")]
        [JsonPropertyName("addr")]
        public string Address { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 和当前坐标点的方向
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 离坐标点距离
        /// </summary>
        public string Distance { get; set; }
        /// <summary>
        /// poi坐标{x,y}
        /// </summary>
        public Point Point { get; set; } = new Point();

        /// <summary>
        /// 坐标类型
        /// </summary>
        public string PoiType { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [JsonProperty("tel")]
        [JsonPropertyName("tel")]
        public string TelPhone { get; set; }
        /// <summary>
        /// poi唯一标识
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [JsonProperty("zip")]
        [JsonPropertyName("zip")]
        public string Post { get; set; }
        /// <summary>
        /// poi对应的主点poi（如，海底捞的主点为上地华联，该字段则为上地华联的poi信息。
        /// 如无，该字段为空），包含子字段和pois基础召回字段相同。
        /// </summary>
        [JsonProperty("parent_poi")]
        [JsonPropertyName("parent_poi")]
        public BaiduPoi ParentPoi { get; set; }
    }
}

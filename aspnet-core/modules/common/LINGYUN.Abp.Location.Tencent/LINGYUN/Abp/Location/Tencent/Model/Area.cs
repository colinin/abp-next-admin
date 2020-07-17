using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 区域信息
    /// </summary>
    public class Area
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
        /// 坐标
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; } = new Location();
        /// <summary>
        /// 此参考位置到输入坐标的直线距离
        /// </summary>
        [JsonProperty("_distance")]
        public string Distance { get; set; }
        /// <summary>
        /// 此参考位置到输入坐标的方位关系，
        /// 如：北、南、内
        /// </summary>
        [JsonProperty("_dir_desc")]
        public string DirDescription { get; set; }
    }
}

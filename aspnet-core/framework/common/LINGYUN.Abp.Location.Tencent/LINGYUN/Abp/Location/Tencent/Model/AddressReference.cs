using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 坐标相对位置参考
    /// </summary>
    public class AddressReference
    {
        /// <summary>
        /// 商圈
        /// </summary>
        [JsonProperty("business_area")]
        public Area BusinessArea { get; set; } = new Area();
        /// <summary>
        /// 知名区域，如商圈或人们普遍认为有较高知名度的区域
        /// </summary>
        [JsonProperty("famous_area")]
        public Area FamousArea { get; set; } = new Area();
        /// <summary>
        /// 乡镇街道
        /// </summary>
        [JsonProperty("town")]
        public Area Town { get; set; } = new Area();
        /// <summary>
        /// 一级地标，可识别性较强、规模较大的地点、小区等
        /// </summary>
        [JsonProperty("landmark_l1")]
        public Area Landmark1 { get; set; } = new Area();
        /// <summary>
        /// 二级地标，较一级地标更为精确，规模更小
        /// </summary>
        [JsonProperty("landmark_l2")]
        public Area Landmark2 { get; set; } = new Area();
        /// <summary>
        /// 街道
        /// </summary>
        [JsonProperty("street")]
        public Area Street { get; set; } = new Area();
        /// <summary>
        /// 门牌
        /// </summary>
        [JsonProperty("street_number")]
        public Area StreetNumber { get; set; } = new Area();
        /// <summary>
        /// 交叉路口
        /// </summary>
        [JsonProperty("crossroad")]
        public Area CrossRoad { get; set; } = new Area();
        /// <summary>
        /// 水系
        /// </summary>
        [JsonProperty("water")]
        public Area Water { get; set; } = new Area();
    }
}

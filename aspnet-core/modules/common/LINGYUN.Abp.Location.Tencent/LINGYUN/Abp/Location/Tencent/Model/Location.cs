using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 行政区划中心点坐标
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 纬度
        /// </summary>
        [JsonProperty("lat")]
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}

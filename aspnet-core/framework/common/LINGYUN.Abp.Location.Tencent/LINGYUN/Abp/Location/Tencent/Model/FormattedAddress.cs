using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Model
{
    /// <summary>
    /// 位置描述
    /// </summary>
    public class FormattedAddress
    {
        /// <summary>
        /// 经过腾讯地图优化过的描述方式，更具人性化特点
        /// </summary>
        [JsonProperty("recommend")]
        public string ReCommend { get; set; }
        /// <summary>
        /// 大致位置，可用于对位置的粗略描述
        /// </summary>
        [JsonProperty("rough")]
        public string Rough { get; set; }
    }
}

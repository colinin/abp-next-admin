using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Amap
{
    /// <summary>
    /// 正地址解析结果
    /// </summary>
    public class AmapPositiveHttpResponse : AmapHttpResponse
    {
        /// <summary>
        /// 返回结果数目
        /// </summary>
        /// <remarks>
        /// 返回结果的个数。
        /// </remarks>
        public int Count { get; set; }

        /// <summary>
        /// 地理编码信息列表
        /// </summary>
        [JsonProperty("geocodes")]
        public AmapGeocode[] Geocodes { get; set; }

        public AmapPositiveHttpResponse()
        {
            Geocodes = new AmapGeocode[0];
        }
    }
}

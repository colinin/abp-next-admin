using LINGYUN.Abp.Location.Tencent.Model;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Response
{
    public class TencentReGeocodeResponse : TencentLocationResponse
    {
        /// <summary>
        /// 逆地址解析结果
        /// </summary>
        [JsonProperty("result")]
        public TencentReGeocode Result { get; set; } = new TencentReGeocode();
    }
}

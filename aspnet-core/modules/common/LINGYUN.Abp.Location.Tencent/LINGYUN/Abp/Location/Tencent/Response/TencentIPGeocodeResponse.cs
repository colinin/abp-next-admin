using LINGYUN.Abp.Location.Tencent.Model;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Location.Tencent.Response
{
    public class TencentIPGeocodeResponse : TencentLocationResponse
    {
        /// <summary>
        /// IP定位结果
        /// </summary>
        [JsonProperty("result")]
        public TencentIPGeocode Result { get; set; } = new TencentIPGeocode();
    }
}

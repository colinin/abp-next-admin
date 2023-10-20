using LINGYUN.Abp.Location.Baidu.Model;

namespace LINGYUN.Abp.Location.Baidu.Response
{
    public class BaiduReGeocodeResponse : BaiduLocationResponse
    {
        public BaiduReGeocode Result { get; set; } = new BaiduReGeocode();
    }
}

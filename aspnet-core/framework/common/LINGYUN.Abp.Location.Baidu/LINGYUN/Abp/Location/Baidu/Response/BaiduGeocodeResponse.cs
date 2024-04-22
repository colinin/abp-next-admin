using LINGYUN.Abp.Location.Baidu.Model;

namespace LINGYUN.Abp.Location.Baidu.Response
{
    public class BaiduGeocodeResponse : BaiduLocationResponse
    {
        public BaiduGeocode Result { get; set; } = new BaiduGeocode();
    }
}

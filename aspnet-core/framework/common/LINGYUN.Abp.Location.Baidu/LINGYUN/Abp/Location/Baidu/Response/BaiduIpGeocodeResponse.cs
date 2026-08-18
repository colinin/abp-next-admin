using LINGYUN.Abp.Location.Baidu.Model;

namespace LINGYUN.Abp.Location.Baidu.Response;

public class BaiduIpGeocodeResponse : BaiduLocationResponse
{
    public string Address { get; set; } = default!;

    public Content Content { get; set; } = new Content();
}

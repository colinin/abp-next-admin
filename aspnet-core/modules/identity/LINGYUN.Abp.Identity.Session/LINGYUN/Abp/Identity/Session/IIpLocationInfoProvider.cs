using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session;
public interface IIpLocationInfoProvider
{
    /// <summary>
    /// 通过ip地址获取地理信息
    /// </summary>
    /// <param name="ipAddress">ip地址</param>
    /// <returns>
    /// 如果解析成功返回地理信息,否则返回null
    /// </returns>
    Task<LocationInfo> GetLocationInfoAsync(string ipAddress);
}

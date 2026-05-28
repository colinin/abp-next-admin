using AlibabaCloud.SDK.Dypnsapi20170525;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sms.Aliyun;

public interface IDypnsClientFactory
{
    /// <summary>
    /// 构建短信认证客户端
    /// </summary>
    /// <returns></returns>
    Task<Client> CreateAsync();
}

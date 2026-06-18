using AlibabaCloud.SDK.Dysmsapi20170525;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sms.Aliyun;

public interface IDysmsClientFactory
{
    /// <summary>
    /// 构建短信客户端
    /// </summary>
    /// <returns></returns>
    Task<Client> CreateAsync();
}

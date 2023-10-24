using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Token;
/// <summary>
/// 获取AccessKey
/// </summary>
public interface IPushPlusTokenProvider
{
    /// <summary>
    ///  获取全局可用Token
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PushPlusToken> GetTokenAsync(CancellationToken cancellationToken = default);
}

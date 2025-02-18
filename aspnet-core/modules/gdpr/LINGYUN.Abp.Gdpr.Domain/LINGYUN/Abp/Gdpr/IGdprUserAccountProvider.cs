using System.Threading.Tasks;

namespace LINGYUN.Abp.Gdpr;
/// <summary>
/// 用户个人账户提供者
/// </summary>
public interface IGdprUserAccountProvider
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 删除用户账户
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task DeleteAsync(GdprDeleteUserAccountContext context);
}

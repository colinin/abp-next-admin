using System.Threading.Tasks;

namespace LINGYUN.Abp.Gdpr;
/// <summary>
/// 用户个人数据提供者<br />
/// 实现: https://abp.io/docs/latest/modules/gdpr#gdpruserdatarequestedeto
/// </summary>
public interface IGdprUserDataProvider
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 准备用户数据
    /// </summary>
    Task PorepareAsync(GdprPrepareUserDataContext context);
    /// <summary>
    /// 删除用户数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task DeleteAsync(GdprDeleteUserDataContext context);
}

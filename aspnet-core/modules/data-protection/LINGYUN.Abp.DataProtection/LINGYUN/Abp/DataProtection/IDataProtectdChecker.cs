using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection
{
    /// <summary>
    /// 实现此接口
    /// 检查资源的访问权限
    /// </summary>
    public interface IDataProtectdChecker
    {
        /// <summary>
        /// 资源是否拥有某种行为的访问权限
        /// </summary>
        /// <typeparam name="T">受保护的资源（实体）</typeparam>
        /// <param name="behavior">访问行为</param>
        /// <returns>不管是否拥有访问权限,请返回非空结果,由EF模块检查</returns>
        Task<ResourceGrantedResult> IsGrantedAsync<T>(ProtectBehavior behavior = ProtectBehavior.All);
    }
}

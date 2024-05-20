using LINGYUN.Abp.DataProtection;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IDataProtectedResourceCache
{
    /// <summary>
    /// 设置指定数据权限的缓存
    /// </summary>
    /// <param name="item">要更新的数据权限缓存项</param>
    void SetCache(DataAccessResource resource);
    /// <summary>
    /// 移除指定主体与实体类型的缓存项
    /// </summary>
    /// <param name="item">要移除的数据权限缓存项信息</param>
    void RemoveCache(DataAccessResource resource);
    /// <summary>
    /// 获取指定主体与实体类型的数据权限过滤规则
    /// </summary>
    /// <param name="subjectName">权限主体</param>
    /// <param name="subjectId">权限主体标识</param>
    /// <param name="entityTypeFullName">实体类型名称</param>
    /// <param name="operation">数据权限操作</param>
    /// <returns>数据过滤条件组</returns>
    DataProtectedResourceCacheItem GetCache(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation);
}

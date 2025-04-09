namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据访问策略
/// </summary>
public enum DataAccessStrategy
{
    /// <summary>
    /// 可以访问所有数据
    /// </summary>
    All,
    /// <summary>
    /// 自定义规则
    /// </summary>
    Custom,
    /// <summary>
    /// 仅当前用户
    /// </summary>
    CurrentUser,
    /// <summary>
    /// 仅当前用户角色
    /// </summary>
    CurrentRoles,
    /// <summary>
    /// 仅当前用户组织机构
    /// </summary>
    CurrentOrganizationUnits,
    /// <summary>
    /// 仅当前用户组织机构及下级机构
    /// </summary>
    CurrentAndSubOrganizationUnits,
}

namespace LINGYUN.Abp.OpenIddict.Impersonation;

public class OpenIddictImpersonationOptions
{
    /// <summary>
    /// 模拟用户时检查权限
    /// </summary>
    public string? ImpersonationPermission {  get; set; }
    /// <summary>
    /// 模拟租户时检查权限
    /// </summary>
    public string? ImpersonationTenantPermission { get; set; }
    /// <summary>
    /// 宿主端模拟租户时默认管理员用户名
    /// </summary>
    public string DefaultTenantAdminUserName {  get; set; }
    public OpenIddictImpersonationOptions()
    {
        DefaultTenantAdminUserName = "admin";
    }
}

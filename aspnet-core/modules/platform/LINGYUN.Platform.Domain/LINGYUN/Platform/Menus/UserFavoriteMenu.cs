using LINGYUN.Platform.Routes;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenu : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid MenuId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    public virtual string AliasName { get; set; }

    public virtual string Color { get; set; }

    public virtual string Framework { get; set; }

    public virtual string Name { get; set; }

    public virtual string DisplayName { get; set; }

    public virtual string Path { get; set; }

    public virtual string Icon { get; set; }

    protected UserFavoriteMenu() { }
    public UserFavoriteMenu(
        Guid id,
        Guid menuId,
        Guid userId,
        string framework,
        string name,
        string displayName,
        string path,
        string icon,
        string color,
        string aliasName = null,
        Guid? tenantId = null)
        : base(id)
    {
        MenuId = menuId;
        UserId = userId;
        Framework = Check.NotNullOrWhiteSpace(framework, nameof(framework), LayoutConsts.MaxFrameworkLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), RouteConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), RouteConsts.MaxDisplayNameLength);
        Path = Check.NotNullOrWhiteSpace(path, nameof(path), RouteConsts.MaxPathLength);
        Icon = Check.Length(icon, nameof(icon), UserFavoriteMenuConsts.MaxIconLength);
        Color = Check.Length(color, nameof(color), UserFavoriteMenuConsts.MaxColorLength);
        AliasName = Check.Length(aliasName, nameof(aliasName), UserFavoriteMenuConsts.MaxAliasNameLength);
        TenantId = tenantId;
    }
}

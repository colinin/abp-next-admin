using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenu : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid MenuId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

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
        Guid? tenantId = null)
        : base(id)
    {
        MenuId = menuId;
        UserId = userId;
        Framework = framework;
        Name = name;
        DisplayName = displayName;
        Path = path;
        Icon = icon;
        TenantId = tenantId;
    }
}

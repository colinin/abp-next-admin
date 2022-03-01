using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Menus
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenu : AuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid MenuId { get; protected set; }

        public virtual string RoleName { get; protected set; }

        public virtual bool Startup { get; set; }

        protected RoleMenu() { }

        public RoleMenu(
            Guid id,
            Guid menuId,
            string roleName,
            Guid? tenantId = null)
            : base(id)
        {
            MenuId = menuId;
            RoleName = roleName;
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { MenuId, RoleName };
        }
    }
}

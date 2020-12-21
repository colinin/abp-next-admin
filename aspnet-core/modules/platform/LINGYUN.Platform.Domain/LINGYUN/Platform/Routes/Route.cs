using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes
{
    /// <summary>
    /// 不管是布局还是视图或者页面，都作为路由的实现，因此抽象一个路由实体<br/> 
    /// 注意:这是基于 Vue Router 的路由规则设计的实体,详情:https://router.vuejs.org/zh/api/#routes
    /// </summary>
    public abstract class Route : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 路径
        /// </summary>
        public virtual string Path { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 重定向路径
        /// </summary>
        public virtual string Redirect { get; set; }

        protected Route() { }

        protected Route(
            [NotNull] Guid id,
            [NotNull] string path,
            [NotNull] string name,
            [NotNull] string displayName,
            [CanBeNull] string redirect = "",
            [CanBeNull] string description = "",
            [CanBeNull] Guid? tenantId = null)
            : base(id)
        {
            Check.NotNullOrWhiteSpace(path, nameof(path));
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(displayName, nameof(displayName));

            Path = path;
            Name = name;
            DisplayName = displayName;
            Redirect = redirect;
            Description = description;
            TenantId = tenantId;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Route route)
            {
                return route.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase);
            }
            return base.Equals(obj);
        }
    }
}

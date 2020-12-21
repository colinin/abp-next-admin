using JetBrains.Annotations;
using LINGYUN.Platform.Routes;
using System;

namespace LINGYUN.Platform.Layouts
{
    /// <summary>
    /// 布局视图实体
    /// </summary>
    public class Layout : Route
    {
        /// <summary>
        /// 所属平台
        /// </summary>
        public virtual PlatformType PlatformType { get; protected set; }
        /// <summary>
        /// 约定的Meta采用哪种数据字典,主要是约束路由必须字段的一致性
        /// </summary>
        public virtual Guid DataId { get; protected set; }

        protected Layout() { }

        public Layout(
            [NotNull] Guid id, 
            [NotNull] string path, 
            [NotNull] string name,
            [NotNull] string displayName,
            [NotNull] Guid dataId,
            [NotNull] PlatformType platformType = PlatformType.None,
            [CanBeNull] string redirect = "",
            [CanBeNull] string description = "",
            [CanBeNull] Guid? tenantId = null) 
            : base(id, path, name, displayName, redirect, description, tenantId)
        {
            DataId = dataId;
            PlatformType = platformType;
        }
    }
}

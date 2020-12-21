using LINGYUN.Platform.Routes;
using System;

namespace LINGYUN.Platform.Layouts
{
    public class LayoutDto : RouteDto
    {
        /// <summary>
        /// 所属平台
        /// </summary>
        public PlatformType PlatformType { get; set; }
        /// <summary>
        /// 约定的Meta采用哪种数据字典,主要是约束路由必须字段的一致性
        /// </summary>
        public Guid DataId { get; set; }
    }
}

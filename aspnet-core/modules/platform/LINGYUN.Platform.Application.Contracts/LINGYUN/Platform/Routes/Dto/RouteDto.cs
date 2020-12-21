using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Routes
{
    public class RouteDto : EntityDto<Guid>
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 重定向路径
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 路由的一些辅助元素,取决于数据字典的设计
        /// </summary>
        public Dictionary<string, object> Meta { get; set; }
    }
}

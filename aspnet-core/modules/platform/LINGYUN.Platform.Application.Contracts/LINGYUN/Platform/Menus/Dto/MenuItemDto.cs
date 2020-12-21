using LINGYUN.Platform.Routes;
using System.Collections.Generic;

namespace LINGYUN.Platform.Menus
{
    public class MenuItemDto : RouteDto
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单组件
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }
}

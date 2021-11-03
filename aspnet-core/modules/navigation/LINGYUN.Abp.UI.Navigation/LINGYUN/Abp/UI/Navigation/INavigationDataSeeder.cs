using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation
{
    public interface INavigationDataSeeder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menus">菜单列表</param>
        /// <param name="multiTenancySides">让用户自行决定是否过滤菜单</param>
        /// <returns></returns>
        Task SeedAsync(
            IReadOnlyCollection<ApplicationMenu> menus,
            MultiTenancySides multiTenancySides = MultiTenancySides.Both);
    }
}

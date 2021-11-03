using System.Threading.Tasks;

namespace LINGYUN.Abp.UI.Navigation
{
    public interface INavigationSeedContributor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task SeedAsync(NavigationSeedContext context);
    }
}

using System.Threading.Tasks;
using Volo.Abp.Data;

namespace PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DataSeeder
{
    public interface IProjectNameDataSeeder
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context">数据种子上下文</param>
        /// <returns>任务</returns>
        Task SeedAsync(DataSeedContext context);
    }
}

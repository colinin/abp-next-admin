using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public class ProjectNameDataSeeder : ITransientDependency
{
    protected ILogger<ProjectNameDataSeeder> Logger { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public ProjectNameDataSeeder(
        ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;

        Logger = NullLogger<ProjectNameDataSeeder>.Instance;
    }

    public virtual Task SeedAsync(DataSeedContext context)
    {
        // TODO: 此处执行你的数据种子
        return Task.CompletedTask;
    }
}

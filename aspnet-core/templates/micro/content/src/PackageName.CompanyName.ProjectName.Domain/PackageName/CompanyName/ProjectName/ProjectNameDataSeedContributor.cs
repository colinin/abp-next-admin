using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace PackageName.CompanyName.ProjectName;

public class ProjectNameDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ILogger<ProjectNameDataSeedContributor> _logger;
    public ProjectNameDataSeedContributor(ILogger<ProjectNameDataSeedContributor> logger)
    {
        _logger = logger;
    }

    public Task SeedAsync(DataSeedContext context)
    {
        _logger.LogInformation("Write your data seed logic here.");

        return Task.CompletedTask;
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;

public class IdentityDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public ILogger<IdentityDataSeedContributor> Logger { protected get; set; }

    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityRoleManager IdentityRoleManager { get; }

    public IdentityDataSeedContributor(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IdentityRoleManager identityRoleManager)
    {
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        IdentityRoleManager = identityRoleManager;

        Logger = NullLogger<IdentityDataSeedContributor>.Instance;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            Logger.LogInformation("Seeding the default role Users...");

            if (await IdentityRoleManager.FindByNameAsync("Users") == null)
            {
                await IdentityRoleManager.CreateAsync(
                    new IdentityRole(
                        GuidGenerator.Create(),
                        "Users",
                        context.TenantId)
                    {
                        IsDefault = true,
                        IsPublic = true,
                        IsStatic = true,
                    });
            }
        }
    }
}

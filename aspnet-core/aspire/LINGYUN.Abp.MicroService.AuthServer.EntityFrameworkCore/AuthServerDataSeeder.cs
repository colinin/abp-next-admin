using LINGYUN.Abp.MicroService.AuthServer.DataSeeds;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.AuthServer;
public class AuthServerDataSeeder : ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected OpenIddictDataSeeder OpenIddictDataSeeder { get; }
    protected IdentityClaimTypeDataSeeder IdentityClaimTypeDataSeeder { get; }
    protected IdentityUserRoleDataSeeder IdentityUserRoleDataSeeder { get; }
    public AuthServerDataSeeder(
        ICurrentTenant currentTenant,
        OpenIddictDataSeeder openIddictDataSeeder,
        IdentityClaimTypeDataSeeder identityClaimTypeDataSeeder, 
        IdentityUserRoleDataSeeder identityUserRoleDataSeeder)
    {
        CurrentTenant = currentTenant;
        OpenIddictDataSeeder = openIddictDataSeeder;
        IdentityClaimTypeDataSeeder = identityClaimTypeDataSeeder;
        IdentityUserRoleDataSeeder = identityUserRoleDataSeeder;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            await OpenIddictDataSeeder.SeedAsync(context);
            await IdentityClaimTypeDataSeeder.SeedAsync(context);
            await IdentityUserRoleDataSeeder.SeedAsync(context);
        }
    }
}

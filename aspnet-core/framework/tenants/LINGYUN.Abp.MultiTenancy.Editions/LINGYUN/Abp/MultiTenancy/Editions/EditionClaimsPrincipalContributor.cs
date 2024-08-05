using LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.MultiTenancy.Editions;

public class EditionClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public async virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<EditionsFeature>())
        {
            return;
        }

        var currentTenant = context.GetRequiredService<ICurrentTenant>();
        if (!currentTenant.IsAvailable)
        {
            return;
        }

        var claimsIdentity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (claimsIdentity.FindAll(x => x.Type == AbpClaimTypes.EditionId).Any())
        {
            return;
        }

        var editionConfigurationProvider = context.GetRequiredService<IEditionConfigurationProvider>();
        var edition = await editionConfigurationProvider.GetAsync(currentTenant.Id);
        if (edition == null)
        {
            return;
        }

        claimsIdentity.AddOrReplace(new Claim(AbpClaimTypes.EditionId, edition.Id.ToString()));

        context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
    }
}

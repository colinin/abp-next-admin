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
    // https://github.com/dotnet/aspnetcore/blob/v5.0.0/src/Identity/Extensions.Core/src/UserClaimsPrincipalFactory.cs#L79
    private static string IdentityAuthenticationType => "Identity.Application";

    protected ICurrentTenant CurrentTenant { get; }
    protected IEditionConfigurationProvider EditionConfigurationProvider { get; }

    public EditionClaimsPrincipalContributor(
        ICurrentTenant currentTenant,
        IEditionConfigurationProvider editionConfigurationProvider)
    {
        CurrentTenant = currentTenant;
        EditionConfigurationProvider = editionConfigurationProvider;
    }

    public async virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<EditionsFeature>())
        {
            return;
        }

        if (!CurrentTenant.IsAvailable)
        {
            return;
        }

        var edition = await EditionConfigurationProvider.GetAsync(CurrentTenant.Id);
        if (edition == null)
        {
            return;
        }

        var claimsIdentity = context.ClaimsPrincipal.Identities.First(x => x.AuthenticationType == IdentityAuthenticationType);

        claimsIdentity.AddOrReplace(new Claim(AbpClaimTypes.EditionId, edition.Id.ToString()));

        context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
    }
}

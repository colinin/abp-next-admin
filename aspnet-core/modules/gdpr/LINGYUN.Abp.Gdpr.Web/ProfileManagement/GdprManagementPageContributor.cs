using LINGYUN.Abp.Gdpr.Localization;
using LINGYUN.Abp.Gdpr.Web.Pages.Account.Components.ProfileManagementGroup.Gdpr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.ProfileManagement;

namespace LINGYUN.Abp.Gdpr.Web.ProfileManagement;

public class GdprManagementPageContributor : IProfileManagementPageContributor
{
    public virtual Task ConfigureAsync(ProfileManagementPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<GdprResource>>();

        context.Groups.Add(
            new ProfileManagementPageGroup(
                "LINGYUN.Abp.Account.Gdpr",
                l["PersonalData"],
                typeof(AccountProfileGdprManagementGroupViewComponent)
            )
        );

        return Task.CompletedTask;
    }
}

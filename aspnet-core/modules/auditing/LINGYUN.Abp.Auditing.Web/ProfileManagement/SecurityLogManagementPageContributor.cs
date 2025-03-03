using LINGYUN.Abp.Auditing.Web.Pages.Account.Components.ProfileManagementGroup.SecurityLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AuditLogging.Localization;

namespace LINGYUN.Abp.Auditing.Web.ProfileManagement;

public class SecurityLogManagementPageContributor : IProfileManagementPageContributor
{
    public virtual Task ConfigureAsync(ProfileManagementPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AuditLoggingResource>>();

        context.Groups.Add(
            new ProfileManagementPageGroup(
                "LINGYUN.Abp.Account.SecurityLog",
                l["SecurityLog"],
                typeof(AccountProfileSecurityLogManagementGroupViewComponent)
            )
        );

        return Task.CompletedTask;
    }
}

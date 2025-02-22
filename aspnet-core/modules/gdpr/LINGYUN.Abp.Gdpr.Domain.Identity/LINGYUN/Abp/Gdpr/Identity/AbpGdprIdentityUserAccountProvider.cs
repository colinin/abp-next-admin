using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Gdpr.Identity;

/// <summary>
/// 身份标识模块用户账户提供者
/// </summary>
public class AbpGdprIdentityUserAccountProvider : GdprUserAccountProviderBase
{
    public const string ProviderName = "Identity";

    public override string Name => ProviderName;

    public async override Task DeleteAsync(GdprDeleteUserAccountContext context)
    {
        var identityUserManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();

        var identityUser = await identityUserManager.GetByIdAsync(context.UserId);

        (await identityUserManager.DeleteAsync(identityUser)).CheckErrors();
    }
}

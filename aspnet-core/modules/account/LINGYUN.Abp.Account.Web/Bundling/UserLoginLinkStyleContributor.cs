using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace LINGYUN.Abp.Account.Web.Bundling;

public class UserLoginLinkStyleContributor : BundleContributor
{
    public override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        context.Files.Add("/styles/user-login-link/fix-style.css");

        return Task.CompletedTask;
    }
}

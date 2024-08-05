using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;

namespace LINGYUN.Abp.OpenIddict.AspNetCore;
public class AbpSessionOpenIddictClaimsPrincipalHandler : IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        return Task.CompletedTask;
    }
}

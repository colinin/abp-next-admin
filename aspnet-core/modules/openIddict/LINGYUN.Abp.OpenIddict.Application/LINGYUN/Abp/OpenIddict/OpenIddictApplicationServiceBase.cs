using System.Threading;
using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OpenIddict;

public abstract class OpenIddictApplicationServiceBase : ApplicationService
{
    protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();
    protected ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.LazyGetRequiredService<ICancellationTokenProvider>();
    protected OpenIddictApplicationServiceBase()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }

    protected virtual CancellationToken GetCancellationToken()
    {
        return CancellationTokenProvider.FallbackToProvider();
    }
}

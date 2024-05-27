using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Applications;

namespace LINGYUN.Abp.OpenApi.OpenIddict;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
public class OpenIddictAppKeyStore : IAppKeyStore, ITransientDependency
{
    private readonly IAbpOpenIdApplicationStore _appStore;
    private readonly IGuidGenerator _guidGenerator;

    public OpenIddictAppKeyStore(
        IAbpOpenIdApplicationStore appStore,
        IGuidGenerator guidGenerator)
    {
        _appStore = appStore;
        _guidGenerator = guidGenerator;
    }

    public async virtual Task<AppDescriptor> FindAsync(string appKey, CancellationToken cancellationToken = default)
    {
        var application = await _appStore.FindByClientIdAsync(appKey, cancellationToken);
        if (application != null)
        {
            int? signLifeTime = null;

            var signLifeTimeProp = application.GetProperty(nameof(AppDescriptor.SignLifetime));
            if (signLifeTimeProp != null && int.TryParse(signLifeTimeProp.ToString(), out var time))
            {
                signLifeTime = time;
            }

            return new AppDescriptor(application.DisplayName, application.ClientId, application.ClientSecret, signLifeTime: signLifeTime);
        }

        return null;
    }

    public async virtual Task StoreAsync(AppDescriptor descriptor, CancellationToken cancellationToken = default)
    {
        var application = new OpenIddictApplicationModel
        {
            Id = _guidGenerator.Create(),
            ClientId = descriptor.AppKey,
            ClientSecret = descriptor.AppSecret,
            DisplayName = descriptor.AppName,
        };
        if (descriptor.SignLifetime.HasValue)
        {
            application.SetProperty(nameof(AppDescriptor.SignLifetime), descriptor.SignLifetime);
        }
        await _appStore.CreateAsync(application, cancellationToken);
    }
}

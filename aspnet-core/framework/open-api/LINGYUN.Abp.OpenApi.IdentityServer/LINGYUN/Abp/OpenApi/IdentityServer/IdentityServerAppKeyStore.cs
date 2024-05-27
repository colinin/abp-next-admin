using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.OpenApi.IdentityServer;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
public class IdentityServerAppKeyStore : IAppKeyStore, ITransientDependency
{
    public ILogger<IdentityServerAppKeyStore> Logger { protected get; set; }

    private readonly IGuidGenerator _guidGenerator;
    private readonly IClientRepository _clientRepository;

    public IdentityServerAppKeyStore(
        IGuidGenerator guidGenerator,
        IClientRepository clientRepository)
    {
        _guidGenerator = guidGenerator;
        _clientRepository = clientRepository;

        Logger = NullLogger<IdentityServerAppKeyStore>.Instance;
    }

    public async virtual Task<AppDescriptor> FindAsync(string appKey, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.FindByClientIdAsync(appKey, cancellationToken: cancellationToken);
        if (client != null)
        {
            int? signLifeTime = null;

            var appSecret = client.FindSecret(nameof(AppDescriptor.AppSecret));
            if (appSecret == null)
            {
                Logger.LogWarning("Found a client {ClientId} that meets the criteria, but did not specify the client key [AppSecret]", client.ClientId);
                return null;
            }

            var signLifeTimeProp = client.FindProperty(nameof(AppDescriptor.SignLifetime));
            if (signLifeTimeProp != null && int.TryParse(signLifeTimeProp.Value, out var time))
            {
                signLifeTime = time;
            }

            return new AppDescriptor(client.ClientName, client.ClientId, appSecret.Value, signLifeTime: signLifeTime);
        }

        return null;
    }

    public async virtual Task StoreAsync(AppDescriptor descriptor, CancellationToken cancellationToken = default)
    {
        var client = new Client(_guidGenerator.Create(), descriptor.AppKey)
        {
            ClientName = descriptor.AppName,
        };
        client.AddSecret(descriptor.AppSecret);
        if (descriptor.SignLifetime.HasValue)
        {
            client.AddProperty(nameof(AppDescriptor.SignLifetime), descriptor.SignLifetime.Value.ToString());
        }

        await _clientRepository.InsertAsync(client, cancellationToken: cancellationToken);
    }
}

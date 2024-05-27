using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OpenApi.ConfigurationStore
{
    [Dependency(TryRegister = true)]
    public class DefaultAppKeyStore : IAppKeyStore, ITransientDependency
    {
        private readonly AbpDefaultAppKeyStoreOptions _options;

        public DefaultAppKeyStore(IOptionsMonitor<AbpDefaultAppKeyStoreOptions> options)
        {
            _options = options.CurrentValue;
        }

        public Task<AppDescriptor> FindAsync(string appKey, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Find(appKey));
        }

        public AppDescriptor Find(string appKey)
        {
            return _options.AppDescriptors?.FirstOrDefault(t => t.AppKey == appKey);
        }

        public Task StoreAsync(AppDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            _options.AppDescriptors.AddIfNotContains(descriptor);
            return Task.CompletedTask;
        }
    }
}

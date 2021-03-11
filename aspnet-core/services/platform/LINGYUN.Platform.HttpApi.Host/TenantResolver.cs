using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform
{
    [Dependency(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(ITenantResolver))]
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AbpTenantResolveOptions _options;

        public TenantResolver(IOptions<AbpTenantResolveOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public virtual async Task<TenantResolveResult> ResolveTenantIdOrNameAsync()
        {
            var result = new TenantResolveResult();

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in _options.TenantResolvers)
                {
                    await tenantResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(tenantResolver.Name);

                    if (context.Handled || context.TenantIdOrName != null)
                    {
                        result.TenantIdOrName = context.TenantIdOrName;
                        break;
                    }
                }
            }

            return result;
        }
    }
}

using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer;
public class AbpEventService : IEventService
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpIdentityServerEventOptions> Options { get; }

    public AbpEventService(
        IServiceScopeFactory serviceScopeFactory, 
        IOptions<AbpIdentityServerEventOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options;
    }

    public virtual bool CanRaiseEventType(EventTypes evtType)
    {
        return true;
    }

    public async virtual Task RaiseAsync(Event evt)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var providerType in Options.Value.EventServiceHandlers)
            {
                var provider = (IAbpIdentityServerEventServiceHandler)scope.ServiceProvider.GetRequiredService(providerType);
                await provider.RaiseAsync(evt);
            }
        }
    }
}

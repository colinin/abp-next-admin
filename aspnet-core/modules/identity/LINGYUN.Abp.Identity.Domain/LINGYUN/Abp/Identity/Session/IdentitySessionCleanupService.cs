using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Identity.Session;
/// <summary>
/// 用户会话清理服务
/// </summary>
public class IdentitySessionCleanupService : ITransientDependency
{
    public ILogger<IdentitySessionCleanupService> Logger { protected get; set; }

    protected IdentitySessionCleanupOptions Options { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }
    public IdentitySessionCleanupService(
        IOptions<IdentitySessionCleanupOptions> options,
        IIdentitySessionStore identitySessionStore)
    {
        Options = options.Value;
        IdentitySessionStore = identitySessionStore;

        Logger = NullLogger<IdentitySessionCleanupService>.Instance;
    }

    [UnitOfWork]
    public async virtual Task CleanAsync()
    {
        Logger.LogDebug($"Cleaning inactive user session.");

        await IdentitySessionStore.RevokeAllAsync(Options.InactiveTimeSpan);

        Logger.LogDebug($"Cleaned inactive user session.");
    }
}

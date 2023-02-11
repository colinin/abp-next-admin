using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement;

public class OssObjectTempCleanupService : ITransientDependency
{
    public ILogger<OssObjectTempCleanupService> Logger { get; set; }
    public IOssObjectExpireor OssObjectExpireor { get; set; }
    protected AbpOssManagementOptions Options { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public OssObjectTempCleanupService(
        ICurrentTenant currentTenant,
        IOptions<AbpOssManagementOptions> options)
    {
        CurrentTenant = currentTenant;
        Options = options.Value;

        OssObjectExpireor = NullOssObjectExpireor.Instance;
        Logger = NullLogger<OssObjectTempCleanupService>.Instance;
    }

    public virtual async Task CleanAsync()
    {
        Logger.LogInformation("Start cleanup.");

        if (!Options.DisableTempPruning)
        {
            var host = CurrentTenant.IsAvailable ? CurrentTenant.Name ?? CurrentTenant.Id.ToString() : "host";

            Logger.LogInformation($"Start cleanup {host} temp objects.");

            var threshold = DateTimeOffset.UtcNow - Options.MinimumTempLifeSpan;

            try
            {
                var request = new ExprieOssObjectRequest(
                    "temp",
                    Options.MaximumTempSize,
                    threshold);

                await OssObjectExpireor.ExpireAsync(request);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}

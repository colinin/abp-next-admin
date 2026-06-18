using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContentTypeResolver : IBlobContentTypeResolver, ITransientDependency
{
    public ILogger<BlobContentTypeResolver> Logger { get; set; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected AbpBlobManagementContentTypeResolveOptions Options { get; }

    public BlobContentTypeResolver(
        IOptions<AbpBlobManagementContentTypeResolveOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;

        Logger = NullLogger<BlobContentTypeResolver>.Instance;
    }

    public async virtual Task<string> ResolveContentTypeAsync(string blobName, Stream blobStream)
    {
        string? result = null;

        Logger.LogDebug("Starting resolving content type...");
        using (var serviceScope = ServiceScopeFactory.CreateScope())
        {
            var context = new BlobContentTypeResolveContext(serviceScope.ServiceProvider, blobName, blobStream);

            foreach (var blobContentTypeResolver in Options.BlobContentTypeResolvers.OrderByDescending(x => x.Priority))
            {
                var originalPosition = context.Stream.CanSeek ? context.Stream.Position : -1;

                Logger.LogDebug("Trying to resolve content type '{Name}'...", blobContentTypeResolver.Name);

                await blobContentTypeResolver.ResolveAsync(context);

                if (originalPosition >= 0 && context.Stream.CanSeek)
                {
                    context.Stream.Position = originalPosition;
                }

                if (context.HasResolvedContentType())
                {
                    result = context.ContentType;

                    Logger.LogDebug("Content type resolved by '{Name}' as '{ContentType}'.", blobContentTypeResolver.Name, result);
                    break;
                }
            }
        }

        if (result.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("No provider can resolve content type. Using the default \"application/octet-stream\".");

            return "application/octet-stream";
        }

        return result;
    }
}

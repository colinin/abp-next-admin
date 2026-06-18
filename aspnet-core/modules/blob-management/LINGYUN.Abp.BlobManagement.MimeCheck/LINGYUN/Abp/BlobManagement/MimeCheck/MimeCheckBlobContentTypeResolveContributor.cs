using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimeCheck.Detection;
using MimeCheck.Validation;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement.MimeCheck;

public class MimeCheckBlobContentTypeResolveContributor : IBlobContentTypeResolveContributor
{
    public string Name => "MimeCheck";
    public int Priority => 200;

    public async virtual Task ResolveAsync(BlobContentTypeResolveContext context)
    {
        try
        {
            var streamResult = await MimeValidator.DetectAsync(context.Stream);
            if (!streamResult.MimeType.IsNullOrWhiteSpace() &&
                !string.Equals(streamResult.MimeType, DetectionResult.Unknown.MimeType))
            {
                context.Success(streamResult.MimeType);
            }
        }
        catch (Exception ex)
        {
            context.ServiceProvider
                .GetService<ILogger<MimeCheckBlobContentTypeResolveContributor>>()
                ?.LogWarning("Failed to decetion the blob mime type: {message}!", ex.Message);
        }
    }
}

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public class FileExtensionBlobContentTypeResolveContributor : IBlobContentTypeResolveContributor
{
    public string Name => "FileExtension";
    public int Priority => 0;

    public virtual Task ResolveAsync(BlobContentTypeResolveContext context)
    {
        var contentTypeProvider = context.ServiceProvider.GetService<IContentTypeProvider>();
        if (contentTypeProvider != null &&
            contentTypeProvider.TryGetContentType(context.BlobName, out var contentType) &&
            !contentType.IsNullOrWhiteSpace())
        {
            context.Success(contentType);
        }
        return Task.CompletedTask;
    }
}

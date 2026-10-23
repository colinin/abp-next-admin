using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.BlobStoring.Features;
using System;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Extensions;

public static class ModuleExtensions
{
    public static IModule UseAbpBlobStoring(this IModule configuration, Action<BlobStoringFeature>? configure = null)
    {
        configuration.Configure(configure);
        return configuration;
    }
}

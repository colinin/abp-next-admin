using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Email.Features;
using System;

namespace LINGYUN.Abp.ElsaNext.Email.Extensions;

public static class ModuleExtensions
{
    public static IModule UseAbpEmail(this IModule configuration, Action<EmailFeature>? configure = null)
    {
        configuration.Configure(configure);
        return configuration;
    }
}

using Elsa.Extensions;
using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.AI.Features;
using System;

namespace LINGYUN.Abp.ElsaNext.AI.Extensions;

public static class ModuleExtensions
{
    public static IModule UseAbpAI(this IModule module, Action<AbpAIFeature>? configure = null)
    {
        return module.Use(configure);
    }
}

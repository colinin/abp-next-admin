using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;

public static class ServiceConfigurationContextExtensions
{
    public static bool IsElsaFeatureEnabled(this ServiceConfigurationContext context, string feature)
    {
        return context.Configuration.GetSection($"Elsa:{feature}:IsEnabled").Get<bool>();
    }
}

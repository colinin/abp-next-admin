using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Saas.Jobs;

internal static class LocalizableStatic
{
    public static ILocalizableString Create(string name)
    {
        return LocalizableString.Create<AbpSaasResource>(name);
    }
}

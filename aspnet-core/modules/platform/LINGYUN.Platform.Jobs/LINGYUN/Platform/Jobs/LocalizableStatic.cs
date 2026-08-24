using LINGYUN.Platform.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Platform.Jobs;

internal static class LocalizableStatic
{
    public static ILocalizableString Create(string name)
    {
        return LocalizableString.Create<PlatformResource>(name);
    }
}

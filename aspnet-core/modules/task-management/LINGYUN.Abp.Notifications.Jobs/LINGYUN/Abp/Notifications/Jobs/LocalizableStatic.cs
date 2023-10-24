using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.Jobs;

internal static class LocalizableStatic
{
    public static ILocalizableString Create(string name)
    {
        return LocalizableString.Create<NotificationsResource>(name);
    }
}

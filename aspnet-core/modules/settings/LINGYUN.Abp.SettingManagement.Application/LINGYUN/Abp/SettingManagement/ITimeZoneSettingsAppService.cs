using IVoloTimeZoneSettingsAppService = Volo.Abp.SettingManagement.ITimeZoneSettingsAppService;

namespace LINGYUN.Abp.SettingManagement;
public interface ITimeZoneSettingsAppService : IVoloTimeZoneSettingsAppService, IUserTimeZoneSettingsAppService
{
}

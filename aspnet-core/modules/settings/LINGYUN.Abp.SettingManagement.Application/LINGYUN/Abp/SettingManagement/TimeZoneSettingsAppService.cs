using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Timing;
using IVoloTimeZoneSettingsAppService = Volo.Abp.SettingManagement.ITimeZoneSettingsAppService;

namespace LINGYUN.Abp.SettingManagement;

[Authorize]
[ExposeServices(
    typeof(ITimeZoneSettingsAppService),
    typeof(IUserTimeZoneSettingsAppService),
    typeof(IVoloTimeZoneSettingsAppService))]
public class TimeZoneSettingsAppService : SettingManagementAppServiceBase, ITimeZoneSettingsAppService
{
    protected ISettingManager SettingManager { get; }
    protected ITimezoneProvider TimezoneProvider { get; }

    private const string UnspecifiedTimeZone = "Unspecified";

    public TimeZoneSettingsAppService(ISettingManager settingManager, ITimezoneProvider timezoneProvider)
    {
        SettingManager = settingManager;
        TimezoneProvider = timezoneProvider;
    }

    public async virtual Task<string> GetMyTimezoneAsync()
    {
        return await SettingManager.GetOrNullForCurrentUserAsync(TimingSettingNames.TimeZone)
            ?? UnspecifiedTimeZone;
    }

    public async virtual Task SetMyTimezoneAsync(string timezone)
    {
        if (timezone.Equals(UnspecifiedTimeZone, StringComparison.OrdinalIgnoreCase))
        {
            timezone = null;
        }

        await SettingManager.SetForCurrentUserAsync(TimingSettingNames.TimeZone, timezone);
    }

    [Authorize(Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone)]
    public async virtual Task<string> GetAsync()
    {
        var timezone = CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host
            ? await SettingManager.GetOrNullGlobalAsync(TimingSettingNames.TimeZone)
            : await SettingManager.GetOrNullForCurrentTenantAsync(TimingSettingNames.TimeZone);

        if (timezone.IsNullOrWhiteSpace())
        {
            timezone = UnspecifiedTimeZone;
        }

        return timezone;
    }

    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        var timezones = TimeZoneHelper.GetTimezones(TimezoneProvider.GetIanaTimezones());
        timezones.Insert(0, new NameValue
        {
            Name = L["DefaultTimeZone"],
            Value = UnspecifiedTimeZone
        });
        return Task.FromResult(timezones);
    }

    [Authorize(Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone)]
    public async virtual Task UpdateAsync(string timezone)
    {
        if (timezone.Equals(UnspecifiedTimeZone, StringComparison.OrdinalIgnoreCase))
        {
            timezone = null;
        }

        if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host)
        {
            await SettingManager.SetGlobalAsync(TimingSettingNames.TimeZone, timezone);
        }
        else
        {
            await SettingManager.SetForCurrentTenantAsync(TimingSettingNames.TimeZone, timezone);
        }
    }
}

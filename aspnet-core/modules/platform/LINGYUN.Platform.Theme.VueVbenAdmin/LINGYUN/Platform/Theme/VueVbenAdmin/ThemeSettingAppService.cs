using LINGYUN.Platform.Localization;
using LINGYUN.Platform.Settings.VueVbenAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Settings;
using Volo.Abp.SettingManagement;
using Microsoft.AspNetCore.Authorization;

namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public class ThemeSettingAppService : ApplicationService, IThemeSettingAppService
{
    protected ISettingManager SettingManager { get; }

    public ThemeSettingAppService(
        ISettingManager settingManager)
    {
        SettingManager = settingManager;

        LocalizationResource = typeof(PlatformResource);
    }

    public async virtual Task<ThemeSettingDto> GetAsync()
    {
        var theme = new ThemeSettingDto();

        var settings = await SettingProvider.GetAllAsync(GetAllSettingNames());

        theme.DarkMode = GetSettingValue(settings, VueVbenAdminSettingNames.DarkMode);

        theme.ProjectConfig.PermissionCacheType = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.PermissionCacheType, 1);
        theme.ProjectConfig.ShowSettingButton = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowSettingButton, true);
        theme.ProjectConfig.ShowDarkModeToggle = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowDarkModeToggle, true);
        theme.ProjectConfig.SettingButtonPosition = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.SettingButtonPosition);
        theme.ProjectConfig.PermissionMode = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.PermissionMode);
        theme.ProjectConfig.SessionTimeoutProcessing = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.SessionTimeoutProcessing, 0);
        theme.ProjectConfig.GrayMode = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.GrayMode, false);
        theme.ProjectConfig.ColorWeak = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ColorWeak, false);
        theme.ProjectConfig.ThemeColor = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ThemeColor);
        theme.ProjectConfig.FullContent = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.FullContent, false);
        theme.ProjectConfig.ContentMode = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ContentMode);
        theme.ProjectConfig.ShowLogo = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowLogo, true);
        theme.ProjectConfig.ShowFooter = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowFooter, false);
        theme.ProjectConfig.OpenKeepAlive = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.OpenKeepAlive, true);
        theme.ProjectConfig.LockTime = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.LockTime, 0);
        theme.ProjectConfig.ShowBreadCrumb = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumb, true);
        theme.ProjectConfig.ShowBreadCrumbIcon = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumbIcon, false);
        theme.ProjectConfig.UseErrorHandle = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.UseErrorHandle, false);
        theme.ProjectConfig.UseOpenBackTop = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.UseOpenBackTop, true);
        theme.ProjectConfig.CanEmbedIFramePage = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.CanEmbedIFramePage, true);
        theme.ProjectConfig.CloseMessageOnSwitch = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.CloseMessageOnSwitch, true);
        theme.ProjectConfig.RemoveAllHttpPending = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.RemoveAllHttpPending, false);

        theme.ProjectConfig.HeaderSetting.BgColor = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.BgColor);
        theme.ProjectConfig.HeaderSetting.Fixed = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Fixed, true);
        theme.ProjectConfig.HeaderSetting.Show = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Show, true);
        theme.ProjectConfig.HeaderSetting.Theme = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Theme);
        theme.ProjectConfig.HeaderSetting.ShowFullScreen = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowFullScreen, true);
        theme.ProjectConfig.HeaderSetting.UseLockPage = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.UseLockPage, true);
        theme.ProjectConfig.HeaderSetting.ShowDoc = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowDoc, true);
        theme.ProjectConfig.HeaderSetting.ShowNotice = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowNotice, true);
        theme.ProjectConfig.HeaderSetting.ShowSearch = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowSearch, false);

        theme.ProjectConfig.MenuSetting.BgColor = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.BgColor);
        theme.ProjectConfig.MenuSetting.Fixed = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Fixed, true);
        theme.ProjectConfig.MenuSetting.Collapsed = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Collapsed, false);
        theme.ProjectConfig.MenuSetting.CanDrag = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CanDrag, false);
        theme.ProjectConfig.MenuSetting.Show = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Show, true);
        theme.ProjectConfig.MenuSetting.Hidden = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Hidden, false);
        theme.ProjectConfig.MenuSetting.Split = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Split, false);
        theme.ProjectConfig.MenuSetting.MenuWidth = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MenuWidth, 210);
        theme.ProjectConfig.MenuSetting.Mode = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Mode);
        theme.ProjectConfig.MenuSetting.Type = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Type);
        theme.ProjectConfig.MenuSetting.Theme = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Theme);
        theme.ProjectConfig.MenuSetting.Show = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Show, true);
        theme.ProjectConfig.MenuSetting.Theme = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Theme);
        theme.ProjectConfig.MenuSetting.TopMenuAlign = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.TopMenuAlign);
        theme.ProjectConfig.MenuSetting.Trigger = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Trigger);
        theme.ProjectConfig.MenuSetting.Accordion = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Accordion, true);
        theme.ProjectConfig.MenuSetting.CloseMixSidebarOnChange = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CloseMixSidebarOnChange, false);
        theme.ProjectConfig.MenuSetting.CollapsedShowTitle = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CollapsedShowTitle, false);
        theme.ProjectConfig.MenuSetting.MixSideTrigger = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideTrigger);
        theme.ProjectConfig.MenuSetting.MixSideFixed = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideFixed, false);

        theme.ProjectConfig.MultiTabsSetting.Cache = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Cache, false);
        theme.ProjectConfig.MultiTabsSetting.Show = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Show, true);
        theme.ProjectConfig.MultiTabsSetting.ShowQuick = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowQuick, true);
        theme.ProjectConfig.MultiTabsSetting.CanDrag = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.CanDrag, true);
        theme.ProjectConfig.MultiTabsSetting.ShowRedo = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowRedo, true);
        theme.ProjectConfig.MultiTabsSetting.ShowFold = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowFold, true);

        theme.ProjectConfig.TransitionSetting.Enable = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.Enable, true);
        theme.ProjectConfig.TransitionSetting.BasicTransition = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.BasicTransition);
        theme.ProjectConfig.TransitionSetting.OpenPageLoading = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenPageLoading, true);
        theme.ProjectConfig.TransitionSetting.OpenNProgress = GetSettingValue(settings, VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenNProgress, false);

        // 忽略
        //theme.BeforeMiniInfo.MenuCollapsed = GetSettingValue(settings, VueVbenAdminSettingNames.BeforeMiniInfo.MenuCollapsed)?.To<bool>();
        //theme.BeforeMiniInfo.MenuSplit = GetSettingValue(settings, VueVbenAdminSettingNames.BeforeMiniInfo.MenuSplit)?.To<bool>();
        //theme.BeforeMiniInfo.MenuMode = GetSettingValue(settings, VueVbenAdminSettingNames.BeforeMiniInfo.MenuMode);
        //theme.BeforeMiniInfo.MenuType = GetSettingValue(settings, VueVbenAdminSettingNames.BeforeMiniInfo.MenuType);

        return theme;
    }

    [Authorize]
    public async virtual Task ChangeAsync(ThemeSettingDto input)
    {
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.DarkMode, input.DarkMode);

        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.PermissionCacheType, input.ProjectConfig.PermissionCacheType.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowSettingButton, input.ProjectConfig.ShowSettingButton.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowDarkModeToggle, input.ProjectConfig.ShowDarkModeToggle.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.SettingButtonPosition, input.ProjectConfig.SettingButtonPosition);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.PermissionMode, input.ProjectConfig.PermissionMode);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.SessionTimeoutProcessing, input.ProjectConfig.SessionTimeoutProcessing.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.GrayMode, input.ProjectConfig.GrayMode.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ColorWeak, input.ProjectConfig.ColorWeak.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ThemeColor, input.ProjectConfig.ThemeColor);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.FullContent, input.ProjectConfig.FullContent.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ContentMode, input.ProjectConfig.ContentMode);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowLogo, input.ProjectConfig.ShowLogo.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowFooter, input.ProjectConfig.ShowFooter.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.OpenKeepAlive, input.ProjectConfig.OpenKeepAlive.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.LockTime, input.ProjectConfig.LockTime.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumb, input.ProjectConfig.ShowBreadCrumb.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumbIcon, input.ProjectConfig.ShowBreadCrumbIcon.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.UseErrorHandle, input.ProjectConfig.UseErrorHandle.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.UseOpenBackTop, input.ProjectConfig.UseOpenBackTop.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.CanEmbedIFramePage, input.ProjectConfig.CanEmbedIFramePage.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.CloseMessageOnSwitch, input.ProjectConfig.CloseMessageOnSwitch.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.RemoveAllHttpPending, input.ProjectConfig.RemoveAllHttpPending.ToString());

        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.BgColor, input.ProjectConfig.HeaderSetting.BgColor);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Fixed, input.ProjectConfig.HeaderSetting.Fixed.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Show, input.ProjectConfig.HeaderSetting.Show.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Theme, input.ProjectConfig.HeaderSetting.Theme);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowFullScreen, input.ProjectConfig.HeaderSetting.ShowFullScreen.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.UseLockPage, input.ProjectConfig.HeaderSetting.UseLockPage.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowDoc, input.ProjectConfig.HeaderSetting.ShowDoc.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowNotice, input.ProjectConfig.HeaderSetting.ShowNotice.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowSearch, input.ProjectConfig.HeaderSetting.ShowSearch.ToString());

        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.BgColor, input.ProjectConfig.MenuSetting.BgColor);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Fixed, input.ProjectConfig.MenuSetting.Fixed.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Collapsed, input.ProjectConfig.MenuSetting.Collapsed.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CanDrag, input.ProjectConfig.MenuSetting.CanDrag.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Show, input.ProjectConfig.MenuSetting.Show.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Hidden, input.ProjectConfig.MenuSetting.Hidden.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Split, input.ProjectConfig.MenuSetting.Split.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MenuWidth, input.ProjectConfig.MenuSetting.MenuWidth.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Mode, input.ProjectConfig.MenuSetting.Mode);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Type, input.ProjectConfig.MenuSetting.Type);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Theme, input.ProjectConfig.MenuSetting.Theme);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.TopMenuAlign, input.ProjectConfig.MenuSetting.TopMenuAlign);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Trigger, input.ProjectConfig.MenuSetting.Trigger);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Accordion, input.ProjectConfig.MenuSetting.Accordion.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CloseMixSidebarOnChange, input.ProjectConfig.MenuSetting.CloseMixSidebarOnChange.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CollapsedShowTitle, input.ProjectConfig.MenuSetting.CollapsedShowTitle.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideTrigger, input.ProjectConfig.MenuSetting.MixSideTrigger);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideFixed, input.ProjectConfig.MenuSetting.MixSideFixed.ToString());
       
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Cache, input.ProjectConfig.MultiTabsSetting.Cache.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Show, input.ProjectConfig.MultiTabsSetting.Show.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowQuick, input.ProjectConfig.MultiTabsSetting.ShowQuick.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.CanDrag, input.ProjectConfig.MultiTabsSetting.CanDrag.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowRedo, input.ProjectConfig.MultiTabsSetting.ShowRedo.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowFold, input.ProjectConfig.MultiTabsSetting.ShowFold.ToString());

        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.Enable, input.ProjectConfig.TransitionSetting.Enable.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.BasicTransition, input.ProjectConfig.TransitionSetting.BasicTransition);
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenPageLoading, input.ProjectConfig.TransitionSetting.OpenPageLoading.ToString());
        await SettingManager.SetForCurrentUserAsync(VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenNProgress, input.ProjectConfig.TransitionSetting.OpenNProgress.ToString());
    }

    protected virtual string[] GetAllSettingNames()
    {
        return new string[]
        {
            VueVbenAdminSettingNames.DarkMode,

            VueVbenAdminSettingNames.ProjectConfig.PermissionCacheType,
            VueVbenAdminSettingNames.ProjectConfig.ShowSettingButton,
            VueVbenAdminSettingNames.ProjectConfig.ShowDarkModeToggle,
            VueVbenAdminSettingNames.ProjectConfig.SettingButtonPosition,
            VueVbenAdminSettingNames.ProjectConfig.PermissionMode,
            VueVbenAdminSettingNames.ProjectConfig.SessionTimeoutProcessing,
            VueVbenAdminSettingNames.ProjectConfig.GrayMode,
            VueVbenAdminSettingNames.ProjectConfig.ColorWeak,
            VueVbenAdminSettingNames.ProjectConfig.ThemeColor,
            VueVbenAdminSettingNames.ProjectConfig.FullContent,
            VueVbenAdminSettingNames.ProjectConfig.ContentMode,
            VueVbenAdminSettingNames.ProjectConfig.ShowLogo,
            VueVbenAdminSettingNames.ProjectConfig.ShowFooter,
            VueVbenAdminSettingNames.ProjectConfig.OpenKeepAlive,
            VueVbenAdminSettingNames.ProjectConfig.LockTime,
            VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumb,
            VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumbIcon,
            VueVbenAdminSettingNames.ProjectConfig.UseErrorHandle,
            VueVbenAdminSettingNames.ProjectConfig.UseOpenBackTop,
            VueVbenAdminSettingNames.ProjectConfig.CanEmbedIFramePage,
            VueVbenAdminSettingNames.ProjectConfig.CloseMessageOnSwitch,
            VueVbenAdminSettingNames.ProjectConfig.RemoveAllHttpPending,

            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.BgColor,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Fixed,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Show,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Theme,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowFullScreen,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.UseLockPage,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowDoc,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowNotice,
            VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowSearch,

            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.BgColor,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Fixed,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Collapsed,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CanDrag,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Show,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Hidden,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Split,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MenuWidth,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Mode,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Type,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Theme,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.TopMenuAlign,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Trigger,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Accordion,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CloseMixSidebarOnChange,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CollapsedShowTitle,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideTrigger,
            VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideFixed,

            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Cache,
            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Show,
            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowQuick,
            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.CanDrag,
            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowRedo,
            VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowFold,

            VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.Enable,
            VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.BasicTransition,
            VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenPageLoading,
            VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenNProgress,
        };
    }

    protected virtual string GetSettingValue(IEnumerable<SettingValue> settings, string name, string defaultValue = null)
    {
        var settingValue = settings.FirstOrDefault(x => x.Name == name)?.Value;

        return settingValue ?? defaultValue;
    }

    protected virtual T GetSettingValue<T>(IEnumerable<SettingValue> settings, string name, T defaultValue = default)
        where T : struct
    {
        var settingValue = settings.FirstOrDefault(x => x.Name == name)?.Value;

        return settingValue?.To<T>() ?? defaultValue;
    }
}

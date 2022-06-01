using LINGYUN.Platform.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Platform.Settings.VueVbenAdmin;

public class VueVbenAdminSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(CreateThemeBasicSettings());
        context.Add(CreateProjectConfigSettings());
        context.Add(CreateHeaderConfigSettings());
        context.Add(CreateMenuConfigSettings());
        context.Add(CreateMultiTabsConfigSettings());
        context.Add(CreateTransitionConfigSettings());
        context.Add(CreateBeforeMiniConfigSettings());
    }

    protected SettingDefinition[] CreateThemeBasicSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.DarkMode,
                defaultValue: "light",
                displayName: L("DisplayName:DarkMode"),
                description: L("Description:DarkMode")),
        };
    }

    protected SettingDefinition[] CreateProjectConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.PermissionCacheType,
                defaultValue: "1",
                displayName: L("DisplayName:PermissionCacheType"),
                description: L("Description:PermissionCacheType")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowSettingButton,
                defaultValue: "true",
                displayName: L("DisplayName:ShowSettingButton"),
                description: L("Description:ShowSettingButton")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowDarkModeToggle,
                defaultValue: "true",
                displayName: L("DisplayName:ShowDarkModeToggle"),
                description: L("Description:ShowDarkModeToggle")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.SettingButtonPosition,
                defaultValue: "auto",
                displayName: L("DisplayName:SettingButtonPosition"),
                description: L("Description:SettingButtonPosition")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.PermissionMode,
                defaultValue: "BACK",
                displayName: L("DisplayName:PermissionMode"),
                description: L("Description:PermissionMode")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.SessionTimeoutProcessing,
                defaultValue: "0",
                displayName: L("DisplayName:SessionTimeoutProcessing"),
                description: L("Description:SessionTimeoutProcessing")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.GrayMode,
                defaultValue: "false",
                displayName: L("DisplayName:GrayMode"),
                description: L("Description:GrayMode")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ColorWeak,
                defaultValue: "false",
                displayName: L("DisplayName:ColorWeak"),
                description: L("Description:ColorWeak")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ThemeColor,
                defaultValue: "#0960bd",
                displayName: L("DisplayName:ThemeColor"),
                description: L("Description:ThemeColor")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.FullContent,
                defaultValue: "false",
                displayName: L("DisplayName:FullContent"),
                description: L("Description:FullContent")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ContentMode,
                defaultValue: "full",
                displayName: L("DisplayName:ContentMode"),
                description: L("Description:ContentMode")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowLogo,
                defaultValue: "true",
                displayName: L("DisplayName:ShowLogo"),
                description: L("Description:ShowLogo")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowFooter,
                defaultValue: "false",
                displayName: L("DisplayName:ShowFooter"),
                description: L("Description:ShowFooter")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.OpenKeepAlive,
                defaultValue: "true",
                displayName: L("DisplayName:OpenKeepAlive"),
                description: L("Description:OpenKeepAlive")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.LockTime,
                defaultValue: "0",
                displayName: L("DisplayName:LockTime"),
                description: L("Description:LockTime")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumb,
                defaultValue: "true",
                displayName: L("DisplayName:ShowBreadCrumb"),
                description: L("Description:ShowBreadCrumb")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.ShowBreadCrumbIcon,
                defaultValue: "false",
                displayName: L("DisplayName:ShowBreadCrumbIcon"),
                description: L("Description:ShowBreadCrumbIcon")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.UseErrorHandle,
                defaultValue: "false",
                displayName: L("DisplayName:UseErrorHandle"),
                description: L("Description:UseErrorHandle")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.UseOpenBackTop,
                defaultValue: "true",
                displayName: L("DisplayName:UseOpenBackTop"),
                description: L("Description:UseOpenBackTop")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.CanEmbedIFramePage,
                defaultValue: "true",
                displayName: L("DisplayName:CanEmbedIFramePage"),
                description: L("Description:CanEmbedIFramePage")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.CloseMessageOnSwitch,
                defaultValue: "true",
                displayName: L("DisplayName:CloseMessageOnSwitch"),
                description: L("Description:CloseMessageOnSwitch")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.RemoveAllHttpPending,
                defaultValue: "false",
                displayName: L("DisplayName:RemoveAllHttpPending"),
                description: L("Description:RemoveAllHttpPending")),
        };
    }

    protected SettingDefinition[] CreateHeaderConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.BgColor,
                defaultValue: "#ffffff",
                displayName: L("DisplayName:Header_BgColor"),
                description: L("Description:Header_BgColor")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Fixed,
                defaultValue: "true",
                displayName: L("DisplayName:Header_Fixed"),
                description: L("Description:Header_Fixed")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Show,
                defaultValue: "true",
                displayName: L("DisplayName:Header_Show"),
                description: L("Description:Header_Show")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.Theme,
                defaultValue: "light",
                displayName: L("DisplayName:Header_Theme"),
                description: L("Description:Header_Theme")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowFullScreen,
                defaultValue: "true",
                displayName: L("DisplayName:Header_ShowFullScreen"),
                description: L("Description:Header_ShowFullScreen")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.UseLockPage,
                defaultValue: "true",
                displayName: L("DisplayName:Header_UseLockPage"),
                description: L("Description:Header_UseLockPage")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowDoc,
                defaultValue: "true",
                displayName: L("DisplayName:Header_ShowDoc"),
                description: L("Description:Header_ShowDoc")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowNotice,
                defaultValue: "true",
                displayName: L("DisplayName:Header_ShowNotice"),
                description: L("Description:Header_ShowNotice")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.HeaderSetting.ShowSearch,
                defaultValue: "true",
                displayName: L("DisplayName:Header_ShowSearch"),
                description: L("Description:Header_ShowSearch")),
        };
    }

    protected SettingDefinition[] CreateMenuConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.BgColor,
                defaultValue: "#001529",
                displayName: L("DisplayName:Menu_BgColor"),
                description: L("Description:Menu_BgColor")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Fixed,
                defaultValue: "true",
                displayName: L("DisplayName:Menu_Fixed"),
                description: L("Description:Menu_Fixed")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Collapsed,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_Collapsed"),
                description: L("Description:Menu_Collapsed")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CanDrag,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_CanDrag"),
                description: L("Description:Menu_CanDrag")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Show,
                defaultValue: "true",
                displayName: L("DisplayName:Menu_Show"),
                description: L("Description:Menu_Show")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Hidden,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_Hidden"),
                description: L("Description:Menu_Hidden")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Split,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_Split"),
                description: L("Description:Menu_Split")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MenuWidth,
                defaultValue: "210",
                displayName: L("DisplayName:Menu_MenuWidth"),
                description: L("Description:Menu_MenuWidth")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Mode,
                defaultValue: "inline",
                displayName: L("DisplayName:Menu_Mode"),
                description: L("Description:Menu_Mode")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Type,
                defaultValue: "sidebar",
                displayName: L("DisplayName:Menu_Type"),
                description: L("Description:Menu_Type")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Theme,
                defaultValue: "dark",
                displayName: L("DisplayName:Menu_Theme"),
                description: L("Description:Menu_Theme")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.TopMenuAlign,
                defaultValue: "center",
                displayName: L("DisplayName:Menu_TopMenuAlign"),
                description: L("Description:Menu_TopMenuAlign")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Trigger,
                defaultValue: "HEADER",
                displayName: L("DisplayName:Menu_Trigger"),
                description: L("Description:Menu_Trigger")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.Accordion,
                defaultValue: "true",
                displayName: L("DisplayName:Menu_Accordion"),
                description: L("Description:Menu_Accordion")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CloseMixSidebarOnChange,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_CloseMixSidebarOnChange"),
                description: L("Description:Menu_CloseMixSidebarOnChange")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.CollapsedShowTitle,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_CollapsedShowTitle"),
                description: L("Description:Menu_CollapsedShowTitle")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideTrigger,
                defaultValue: "click",
                displayName: L("DisplayName:Menu_MixSideTrigger"),
                description: L("Description:Menu_MixSideTrigger")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MenuSetting.MixSideFixed,
                defaultValue: "false",
                displayName: L("DisplayName:Menu_MixSideFixed"),
                description: L("Description:Menu_MixSideFixed")),
        };
    }

    protected SettingDefinition[] CreateMultiTabsConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Cache,
                defaultValue: "false",
                displayName: L("DisplayName:MultiTabs_Cache"),
                description: L("Description:MultiTabs_Cache")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.Show,
                defaultValue: "true",
                displayName: L("DisplayName:MultiTabs_Show"),
                description: L("Description:MultiTabs_Show")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowQuick,
                defaultValue: "true",
                displayName: L("DisplayName:MultiTabs_ShowQuick"),
                description: L("Description:MultiTabs_ShowQuick")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.CanDrag,
                defaultValue: "true",
                displayName: L("DisplayName:MultiTabs_CanDrag"),
                description: L("Description:MultiTabs_CanDrag")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowRedo,
                defaultValue: "true",
                displayName: L("DisplayName:MultiTabs_ShowRedo"),
                description: L("Description:MultiTabs_ShowRedo")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.MultiTabsSetting.ShowFold,
                defaultValue: "true",
                displayName: L("DisplayName:MultiTabs_ShowFold"),
                description: L("Description:MultiTabs_ShowFold")),
        };
    }

    protected SettingDefinition[] CreateTransitionConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.Enable,
                defaultValue: "true",
                displayName: L("DisplayName:Transition_Enable"),
                description: L("Description:Transition_Enable")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.BasicTransition,
                defaultValue: "fade-slide",
                displayName: L("DisplayName:Transition_BasicTransition"),
                description: L("Description:Transition_BasicTransition")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenPageLoading,
                defaultValue: "true",
                displayName: L("DisplayName:Transition_OpenPageLoading"),
                description: L("Description:Transition_OpenPageLoading")),
            CreateSetting(
                name: VueVbenAdminSettingNames.ProjectConfig.TransitionSetting.OpenNProgress,
                defaultValue: "false",
                displayName: L("DisplayName:Transition_OpenNProgress"),
                description: L("Description:Transition_OpenNProgress")),
        };
    }

    protected SettingDefinition[] CreateBeforeMiniConfigSettings()
    {
        return new SettingDefinition[]
        {
            CreateSetting(
                name: VueVbenAdminSettingNames.BeforeMiniInfo.MenuCollapsed,
                defaultValue: null,
                displayName: L("DisplayName:BeforeMini_MenuCollapsed"),
                description: L("Description:BeforeMini_MenuCollapsed")),
            CreateSetting(
                name: VueVbenAdminSettingNames.BeforeMiniInfo.MenuSplit,
                defaultValue: null,
                displayName: L("DisplayName:BeforeMini_MenuSplit"),
                description: L("Description:BeforeMini_MenuSplit")),
            CreateSetting(
                name: VueVbenAdminSettingNames.BeforeMiniInfo.MenuMode,
                defaultValue: null,
                displayName: L("DisplayName:BeforeMini_MenuMode"),
                description: L("Description:BeforeMini_MenuMode")),
            CreateSetting(
                name: VueVbenAdminSettingNames.BeforeMiniInfo.MenuType,
                defaultValue: null,
                displayName: L("DisplayName:BeforeMini_MenuType"),
                description: L("Description:BeforeMini_MenuType")),
        };
    }

    protected SettingDefinition CreateSetting(
        string name,
        ILocalizableString displayName,
        ILocalizableString description,
        string defaultValue = null,
        bool isVisibleToClients = false,
        bool isInherited = true,
        bool isEncrypted = false)
    {
        return new SettingDefinition(name, defaultValue, displayName, description, isVisibleToClients, isInherited, isEncrypted)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName,
                UserSettingValueProvider.ProviderName);
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<PlatformResource>(name);
    }
}

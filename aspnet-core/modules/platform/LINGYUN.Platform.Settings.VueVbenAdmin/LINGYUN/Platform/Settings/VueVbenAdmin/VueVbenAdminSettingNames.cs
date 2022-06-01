namespace LINGYUN.Platform.Settings.VueVbenAdmin;

public static class VueVbenAdminSettingNames
{
    public const string GroupName = PlatformSettingNames.GroupName + ".Theme.VueVbenAdmin";

    public const string DarkMode = GroupName + ".DarkMode";

    public static class ProjectConfig
    {
        public const string Prefix = GroupName + ".ProjectConfig";
        public const string PermissionCacheType = Prefix + ".PermissionCacheType";
        public const string ShowSettingButton = Prefix + ".ShowSettingButton";
        public const string ShowDarkModeToggle = Prefix + ".ShowDarkModeToggle";
        public const string SettingButtonPosition = Prefix + ".SettingButtonPosition";
        public const string PermissionMode = Prefix + ".PermissionMode";
        public const string SessionTimeoutProcessing = Prefix + ".SessionTimeoutProcessing";
        public const string GrayMode = Prefix + ".GrayMode";
        public const string ColorWeak = Prefix + ".ColorWeak";
        public const string ThemeColor = Prefix + ".ThemeColor";
        public const string FullContent = Prefix + ".FullContent";
        public const string ContentMode = Prefix + ".ContentMode";
        public const string ShowLogo = Prefix + ".ShowLogo";
        public const string ShowFooter = Prefix + ".ShowFooter";
        public const string OpenKeepAlive = Prefix + ".OpenKeepAlive";
        public const string LockTime = Prefix + ".LockTime";
        public const string ShowBreadCrumb = Prefix + ".ShowBreadCrumb";
        public const string ShowBreadCrumbIcon = Prefix + ".ShowBreadCrumbIcon";
        public const string UseErrorHandle = Prefix + ".UseErrorHandle";
        public const string UseOpenBackTop = Prefix + ".UseOpenBackTop";
        public const string CanEmbedIFramePage = Prefix + ".CanEmbedIFramePage";
        public const string CloseMessageOnSwitch = Prefix + ".CloseMessageOnSwitch";
        public const string RemoveAllHttpPending = Prefix + ".RemoveAllHttpPending";

        public static class HeaderSetting
        {
            public const string Prefix = ProjectConfig.Prefix + ".HeaderSetting";
            public const string BgColor = Prefix + ".BgColor";
            public const string Fixed = Prefix + ".Fixed";
            public const string Show = Prefix + ".Show";
            public const string Theme = Prefix + ".Theme";
            public const string ShowFullScreen = Prefix + ".ShowFullScreen";
            public const string UseLockPage = Prefix + ".UseLockPage";
            public const string ShowDoc = Prefix + ".ShowDoc";
            public const string ShowNotice = Prefix + ".ShowNotice";
            public const string ShowSearch = Prefix + ".ShowSearch";
        }

        public static class MenuSetting
        {
            public const string Prefix = ProjectConfig.Prefix + ".MenuSetting";
            public const string BgColor = Prefix + ".BgColor";
            public const string Fixed = Prefix + ".Fixed";
            public const string Collapsed = Prefix + ".Collapsed";
            public const string CanDrag = Prefix + ".CanDrag";
            public const string Show = Prefix + ".Show";
            public const string Hidden = Prefix + ".Hidden";
            public const string Split = Prefix + ".Split";
            public const string MenuWidth = Prefix + ".MenuWidth";
            public const string Mode = Prefix + ".Mode";
            public const string Type = Prefix + ".Type";
            public const string Theme = Prefix + ".Theme";
            public const string TopMenuAlign = Prefix + ".TopMenuAlign";
            public const string Trigger = Prefix + ".Trigger";
            public const string Accordion = Prefix + ".Accordion";
            public const string CloseMixSidebarOnChange = Prefix + ".CloseMixSidebarOnChange";
            public const string CollapsedShowTitle = Prefix + ".CollapsedShowTitle";
            public const string MixSideTrigger = Prefix + ".MixSideTrigger";
            public const string MixSideFixed = Prefix + ".MixSideFixed";
        }

        public static class MultiTabsSetting
        {
            public const string Prefix = ProjectConfig.Prefix + ".MultiTabsSetting";
            public const string Cache = Prefix + ".Cache";
            public const string Show = Prefix + ".Show";
            public const string ShowQuick = Prefix + ".ShowQuick";
            public const string CanDrag = Prefix + ".CanDrag";
            public const string ShowRedo = Prefix + ".ShowRedo";
            public const string ShowFold = Prefix + ".ShowFold";
        }

        public static class TransitionSetting
        {
            public const string Prefix = ProjectConfig.Prefix + ".TransitionSetting";
            public const string Enable = Prefix + ".Enable";
            public const string BasicTransition = Prefix + ".BasicTransition";
            public const string OpenPageLoading = Prefix + ".OpenPageLoading";
            public const string OpenNProgress = Prefix + ".OpenNProgress";
        }
    }

    public static class BeforeMiniInfo
    {
        public const string Prefix = GroupName + ".BeforeMiniInfo";
        public const string MenuCollapsed = Prefix + ".MenuCollapsed";
        public const string MenuSplit = Prefix + ".MenuSplit";
        public const string MenuMode = Prefix + ".MenuMode";
        public const string MenuType = Prefix + ".MenuType";
    }
}

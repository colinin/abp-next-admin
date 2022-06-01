namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public class ProjectConfigDto
{
    public int PermissionCacheType { get; set; } = 1;
    public bool ShowSettingButton { get; set; } = true;
    public bool ShowDarkModeToggle { get; set; } = true;
    public string SettingButtonPosition { get; set; } = "auto";
    public string PermissionMode { get; set; } = "BACK";
    public int SessionTimeoutProcessing { get; set; } = 0;
    public bool GrayMode { get; set; }
    public bool ColorWeak { get; set; }
    public string ThemeColor { get; set; } = "#0960bd";
    public bool FullContent { get; set; }
    public string ContentMode { get; set; } = "full";
    public bool ShowLogo { get; set; } = true;
    public bool ShowFooter { get; set; }
    public HeaderSettingDto HeaderSetting { get; set; } = new HeaderSettingDto();
    public MenuSettingDto MenuSetting { get; set; } = new MenuSettingDto();
    public MultiTabsSettingDto MultiTabsSetting { get; set; } = new MultiTabsSettingDto();
    public TransitionSettingDto TransitionSetting { get; set; } = new TransitionSettingDto();
    public bool OpenKeepAlive { get; set; } = true;
    public int LockTime { get; set; } = 0;
    public bool ShowBreadCrumb { get; set; } = true;
    public bool ShowBreadCrumbIcon { get; set; }
    public bool UseErrorHandle { get; set; }
    public bool UseOpenBackTop { get; set; } = true;
    public bool CanEmbedIFramePage { get; set; } = true;
    public bool CloseMessageOnSwitch { get; set; } = true;
    public bool RemoveAllHttpPending { get; set; }
}

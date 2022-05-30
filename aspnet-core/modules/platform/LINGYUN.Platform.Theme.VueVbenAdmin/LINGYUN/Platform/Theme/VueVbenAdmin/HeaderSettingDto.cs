namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public class HeaderSettingDto
{
    public string BgColor { get; set; } = "#ffffff";
    public bool Fixed { get; set; } = true;
    public bool Show { get; set; } = true;
    public string Theme { get; set; } = "light";
    public bool ShowFullScreen { get; set; } = true;
    public bool UseLockPage { get; set; } = true;
    public bool ShowDoc { get; set; } = true;
    public bool ShowNotice { get; set; } = true;
    public bool ShowSearch { get; set; } = true;
}

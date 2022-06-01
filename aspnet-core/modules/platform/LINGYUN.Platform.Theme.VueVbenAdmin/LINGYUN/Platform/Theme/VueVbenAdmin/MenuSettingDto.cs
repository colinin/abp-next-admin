namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public class MenuSettingDto
{
    public string BgColor { get; set; } = "#001529";
    public bool Fixed { get; set; } = true;
    public bool Collapsed { get; set; }
    public bool CanDrag { get; set; }
    public bool Show { get; set; } = true;
    public bool Hidden { get; set; }
    public bool Split { get; set; }
    public int MenuWidth { get; set; } = 210;
    public string Mode { get; set; } = "inline";
    public string Type { get; set; } = "sidebar";
    public string Theme { get; set; } = "dark";
    public string TopMenuAlign { get; set; } = "center";
    public string Trigger { get; set; } = "HEADER";
    public bool Accordion { get; set; } = true;
    public bool CloseMixSidebarOnChange { get; set; }
    public bool CollapsedShowTitle { get; set; }
    public string MixSideTrigger { get; set; } = "click";
    public bool MixSideFixed { get; set; }
}

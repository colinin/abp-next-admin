namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public class TransitionSettingDto
{
    public bool Enable { get; set; } = true;
    public string BasicTransition { get; set; } = "fade-slide";
    public bool OpenPageLoading { get; set; } = true;
    public bool OpenNProgress { get; set; }
}

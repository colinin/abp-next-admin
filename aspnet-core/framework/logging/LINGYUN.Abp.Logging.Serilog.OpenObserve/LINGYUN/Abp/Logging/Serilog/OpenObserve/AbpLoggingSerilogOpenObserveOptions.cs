namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

public class AbpLoggingSerilogOpenObserveOptions
{
    public string Stream { get; set; }
    public string Organization { get; set; }

    public AbpLoggingSerilogOpenObserveOptions()
    {
        Stream = "default";
        Organization = "default";
    }
}

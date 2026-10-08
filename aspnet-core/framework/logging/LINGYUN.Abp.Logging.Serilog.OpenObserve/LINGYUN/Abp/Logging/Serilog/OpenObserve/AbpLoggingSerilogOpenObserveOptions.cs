namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

public class AbpLoggingSerilogOpenObserveOptions
{
    public string Endpoint { get; set; }
    public string Stream { get; set; }
    public string Organization { get; set; }
    public string? AccessToken { get; set; }

    public AbpLoggingSerilogOpenObserveOptions()
    {
        Endpoint = "http://localhost:5080";
        Stream = "default";
        Organization = "default";
    }
}

using Volo.Abp.Cli.ServiceProxying;

namespace LINGYUN.Abp.Cli.UI;

public class GenerateViewArgs
{
    public GenerateViewArgs(
        string module, 
        string url, 
        string output,
        ServiceType? serviceType = null)
    {
        Module = module;
        Url = url;
        Output = output;
        ServiceType = serviceType;
    }

    public string Module { get; }

    public string Output { get; }

    public string Url { get; }

    public ServiceType? ServiceType { get; }
}

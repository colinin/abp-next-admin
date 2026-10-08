namespace LINGYUN.Abp.Logging;

public class AbpLoggingOptions
{
    public string Provider { get; set; }
    public AbpLoggingOptions()
    {
        Provider = DefaultLoggingProvider.ProviderName;
    }
}

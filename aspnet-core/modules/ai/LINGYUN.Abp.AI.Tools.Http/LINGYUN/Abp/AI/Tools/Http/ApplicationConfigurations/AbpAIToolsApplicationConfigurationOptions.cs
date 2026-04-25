namespace LINGYUN.Abp.AI.Tools.Http.ApplicationConfigurations;
public class AbpAIToolsApplicationConfigurationOptions
{
    public bool IsEnabled { get; set; }
    public string EndPoint { get; set; }
    public AbpAIToolsApplicationConfigurationOptions()
    {
        IsEnabled = false;
        EndPoint = "http://localhost:8080/api/abp/application-configuration";
    }
}

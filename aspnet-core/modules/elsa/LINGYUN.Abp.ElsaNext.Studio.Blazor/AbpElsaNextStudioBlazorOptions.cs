using Elsa.Studio.Models;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor;

public class AbpElsaNextStudioBlazorOptions
{
    public BackendApiConfig BackendApiConfig { get; set; }
    public AbpElsaNextStudioBlazorOptions()
    {
        BackendApiConfig = new BackendApiConfig();
    }
}

using Elsa.Attributes;
using Elsa.Options;
using Elsa.Services.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Feature("BlobStoring")]
public class Startup : StartupBase
{
    public override void ConfigureElsa(ElsaOptionsBuilder elsa, IConfiguration configuration)
    {
        elsa.AddBlobStoringActivities();
    }
}

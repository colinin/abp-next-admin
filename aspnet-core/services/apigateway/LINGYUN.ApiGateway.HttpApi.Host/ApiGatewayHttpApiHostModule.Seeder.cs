using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Threading;

namespace LINGYUN.ApiGateway
{
    public partial class ApiGatewayHttpApiHostModule
    {
        private void SeedData(ApplicationInitializationContext context)
        {
            var configuration = context.GetConfiguration();
            if (configuration.GetSection("ApiGateway:SeedInitScript").Get<bool>())
            {
                AsyncHelper.RunSync(async () =>
                {
                    using var scope = context.ServiceProvider.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
                });
            }
        }
    }
}

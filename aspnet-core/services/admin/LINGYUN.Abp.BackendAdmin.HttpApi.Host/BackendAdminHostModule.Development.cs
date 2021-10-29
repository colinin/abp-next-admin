using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace LINGYUN.Abp.BackendAdmin
{
    public partial class BackendAdminHostModule
    {
        private void UseDevelopment(IApplicationBuilder app, ApplicationInitializationContext context)
        {
            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseProxyConnectTest();
            }
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.BackendAdmin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<BackendAdminHostModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}

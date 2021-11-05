using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LINGYUN.ApiGateway
{
    public partial class ApiGatewayHostModule
    {
        private void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Open API Document");
                var options = app.ApplicationServices.GetRequiredService<IOptions<ApiGatewayOptions>>().Value;
                foreach (var api in options.DownstreamOpenApis)
                {
                    c.SwaggerEndpoint(api.EndPoint, api.Name);
                }
            });

        }
    }
}

using LINGYUN.ApiGateway.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayApplicationServiceBase : ApplicationService
    {
        protected ApiGatewayApplicationServiceBase()
        {
            LocalizationResource = typeof(ApiGatewayResource);
        }
    }
}

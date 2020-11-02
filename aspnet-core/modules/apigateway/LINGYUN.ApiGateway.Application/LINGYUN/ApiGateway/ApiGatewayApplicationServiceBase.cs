using LINGYUN.ApiGateway.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.ApiGateway
{
    public abstract class ApiGatewayApplicationServiceBase : ApplicationService
    {
        protected ApiGatewayApplicationServiceBase()
        {
            LocalizationResource = typeof(ApiGatewayResource);
        }
    }
}

using LINGYUN.ApiGateway.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.ApiGateway
{
    public abstract class ApiGatewayControllerBase : AbpController
    {
        protected ApiGatewayControllerBase()
        {
            LocalizationResource = typeof(ApiGatewayResource);
        }
    }
}

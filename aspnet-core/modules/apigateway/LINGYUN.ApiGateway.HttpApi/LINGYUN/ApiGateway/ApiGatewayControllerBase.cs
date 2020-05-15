using LINGYUN.ApiGateway.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayControllerBase : AbpController
    {
        protected ApiGatewayControllerBase()
        {
            LocalizationResource = typeof(ApiGatewayResource);
        }
    }
}

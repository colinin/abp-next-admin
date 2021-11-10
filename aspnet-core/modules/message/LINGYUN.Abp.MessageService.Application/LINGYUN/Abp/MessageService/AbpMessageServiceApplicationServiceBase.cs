using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService
{
    public abstract class AbpMessageServiceApplicationServiceBase : ApplicationService
    {
        protected AbpMessageServiceApplicationServiceBase()
        {
            LocalizationResource = typeof(MessageServiceResource);
            ObjectMapperContext = typeof(AbpMessageServiceApplicationModule);
        }
    }
}

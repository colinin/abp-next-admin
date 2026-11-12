using LINGYUN.Abp.PrintDesigner.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.PrintDesigner;

public abstract class PrintDesignerAppServiceBase : ApplicationService
{
    protected PrintDesignerAppServiceBase()
    {
        LocalizationResource = typeof(PrintDesignerResource);
        ObjectMapperContext = typeof(AbpPrintDesignerApplicationModule);
    }
}

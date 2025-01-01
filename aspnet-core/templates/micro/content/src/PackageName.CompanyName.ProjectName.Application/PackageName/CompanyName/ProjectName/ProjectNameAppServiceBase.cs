using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.Application.Services;

namespace PackageName.CompanyName.ProjectName;

public abstract class ProjectNameAppServiceBase : ApplicationService
{
    protected ProjectNameAppServiceBase()
    {
        LocalizationResource = typeof(ProjectNameResource);
        ObjectMapperContext = typeof(ProjectNameApplicationModule);
    }
}

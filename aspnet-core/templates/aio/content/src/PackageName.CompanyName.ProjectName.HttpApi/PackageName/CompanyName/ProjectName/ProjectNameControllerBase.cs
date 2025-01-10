using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PackageName.CompanyName.ProjectName;

public abstract class ProjectNameControllerBase : AbpControllerBase
{
    protected ProjectNameControllerBase()
    {
        LocalizationResource = typeof(ProjectNameResource);
    }
}

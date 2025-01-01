using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace PackageName.CompanyName.ProjectName.Features;

public class ProjectNameFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(ProjectNameFeatureNames.GroupName, L("Features:ProjectName"));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<ProjectNameResource>(name);
    }
}

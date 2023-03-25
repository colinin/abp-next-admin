using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Features;
public static class IPermissionDefinitionManagerExtensions
{
    public async static Task<FeatureGroupDefinition> GetGroupOrNullAsync(
        this IFeatureDefinitionManager featureDefinitionManager,
        string name
        )
    {
        var groups = await featureDefinitionManager.GetGroupsAsync();

        return groups.FirstOrDefault(x => x.Name == name);
    }
}

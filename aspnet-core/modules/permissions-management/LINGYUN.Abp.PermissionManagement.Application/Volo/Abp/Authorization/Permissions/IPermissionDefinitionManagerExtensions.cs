using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions;
public static class IPermissionDefinitionManagerExtensions
{
    public async static Task<PermissionGroupDefinition> GetGroupOrNullAsync(
        this IPermissionDefinitionManager permissionDefinitionManager,
        string name
        )
    {
        var groups = await permissionDefinitionManager.GetGroupsAsync();

        return groups.FirstOrDefault(x => x.Name == name);
    }
}

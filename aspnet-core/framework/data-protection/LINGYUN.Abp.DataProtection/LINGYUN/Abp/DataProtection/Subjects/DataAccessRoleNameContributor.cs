using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.Subjects;

public class DataAccessRoleNameContributor : IDataAccessSubjectContributor
{
    public string Name => RolePermissionValueProvider.ProviderName;

    public async virtual Task<List<DataAccessFilterGroup>> GetFilterGroups(DataAccessSubjectContributorContext context)
    {
        var groups = new List<DataAccessFilterGroup>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        if (currentUser.IsAuthenticated)
        {
            var resourceStore = context.ServiceProvider.GetRequiredService<IDataProtectedResourceStore>();
            var roles = currentUser.Roles;
            foreach (var role in roles)
            {
                var resource = await resourceStore.GetAsync(Name, role, context.EntityTypeFullName, context.Operation);
                if (resource?.FilterGroup != null)
                {
                    groups.Add(resource.FilterGroup);
                }
            }
        }
        return groups;
    }

    public async virtual Task<List<string>> GetAccessdProperties(DataAccessSubjectContributorContext context)
    {
        var allowProperties = new List<string>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        if (currentUser.IsAuthenticated)
        {
            var resourceStore = context.ServiceProvider.GetRequiredService<IDataProtectedResourceStore>();
            var roles = currentUser.Roles;
            foreach (var role in roles)
            {
                var resource = await resourceStore.GetAsync(Name, role, context.EntityTypeFullName, context.Operation);
                if (resource?.AccessedProperties.Any() == true)
                {
                    allowProperties.AddIfNotContains(resource.AccessedProperties);
                }
            }
        }
        return allowProperties;
    }
}

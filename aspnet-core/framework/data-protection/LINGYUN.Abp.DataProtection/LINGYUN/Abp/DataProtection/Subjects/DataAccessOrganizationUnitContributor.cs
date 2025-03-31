using LINGYUN.Abp.Authorization.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.Subjects;

public class DataAccessOrganizationUnitContributor : IDataAccessSubjectContributor
{
    public string Name => OrganizationUnitPermissionValueProvider.ProviderName;

    public async virtual Task<List<DataAccessFilterGroup>> GetFilterGroups(DataAccessSubjectContributorContext context)
    {
        var groups = new List<DataAccessFilterGroup>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        if (currentUser.IsAuthenticated)
        {
            var resourceStore = context.ServiceProvider.GetRequiredService<IDataProtectedResourceStore>();
            var orgCodes = currentUser.FindOrganizationUnits();
            foreach (var orgCode in orgCodes)
            {
                var resource = await resourceStore.GetAsync(Name, orgCode, context.EntityTypeFullName, context.Operation);
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
            var orgCodes = currentUser.FindOrganizationUnits();
            foreach (var orgCode in orgCodes)
            {
                var resource = await resourceStore.GetAsync(Name, orgCode, context.EntityTypeFullName, context.Operation);
                if (resource?.AccessedProperties.Any() == true)
                {
                    allowProperties.AddIfNotContains(resource.AccessedProperties);
                }
            }
        }
        return allowProperties;
    }
}

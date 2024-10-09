using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Clients;

namespace LINGYUN.Abp.DataProtection.Subjects;
public class DataAccessClientIdContributor : IDataAccessSubjectContributor
{
    public string Name => ClientPermissionValueProvider.ProviderName;

    public virtual List<string> GetAllowProperties(DataAccessSubjectContributorContext context)
    {
        var allowProperties = new List<string>();
        var currentClient = context.ServiceProvider.GetRequiredService<ICurrentClient>();
        if (currentClient.IsAuthenticated)
        {
            var resourceStore = context.ServiceProvider.GetRequiredService<IDataProtectedResourceStore>();
            var resource = resourceStore.Get(Name, currentClient.Id, context.EntityTypeFullName, context.Operation);
            if (resource?.AllowProperties.Any() == true)
            {
                allowProperties.AddIfNotContains(resource.AllowProperties);
            }
        }
        return allowProperties;
    }

    public virtual List<DataAccessFilterGroup> GetFilterGroups(DataAccessSubjectContributorContext context)
    {
        var groups = new List<DataAccessFilterGroup>();
        var currentClient = context.ServiceProvider.GetRequiredService<ICurrentClient>();
        if (currentClient.IsAuthenticated)
        {
            var resourceStore = context.ServiceProvider.GetRequiredService<IDataProtectedResourceStore>();
            var resource = resourceStore.Get(Name, currentClient.Id, context.EntityTypeFullName, context.Operation);
            if (resource?.FilterGroup != null)
            {
                groups.Add(resource.FilterGroup);
            }
        }
        return groups;
    }
}

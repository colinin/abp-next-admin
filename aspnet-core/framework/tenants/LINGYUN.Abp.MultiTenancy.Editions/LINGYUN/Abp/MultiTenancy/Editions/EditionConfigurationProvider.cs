using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MultiTenancy.Editions;

public class EditionConfigurationProvider : IEditionConfigurationProvider, ITransientDependency
{
    protected virtual IEditionStore EditionStore { get; }

    public EditionConfigurationProvider(IEditionStore editionStore)
    {
        EditionStore = editionStore;
    }

    public async virtual Task<EditionConfiguration> GetAsync(Guid? tenantId = null)
    {
        EditionConfiguration edition = null;
        if (tenantId.HasValue)
        {
            var editionInfo = await EditionStore.FindByTenantAsync(tenantId.Value);
            if (editionInfo != null)
            {
                edition = new EditionConfiguration(
                    editionInfo.Id,
                    editionInfo.DisplayName);
            }
        }

        return edition;
    }
}

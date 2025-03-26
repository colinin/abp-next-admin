using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class AbpDataProtectedWritePropertiesInterceptor : SaveChangesInterceptor, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    public IOptions<AbpDataProtectionOptions> DataProtectionOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDataProtectionOptions>>();
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (DataFilter.IsEnabled<IDataProtected>() && eventData.Context != null)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries().ToList())
            {
                if (entry.State.IsIn(EntityState.Modified))
                {
                    var allowProperties = new List<string>();
                    var entity = entry.Entity;
                    var entityType = entry.Entity.GetType();
                    var subjectContext = new DataAccessSubjectContributorContext(entityType.FullName, DataAccessOperation.Write, LazyServiceProvider);
                    foreach (var contributor in DataProtectionOptions.Value.SubjectContributors)
                    {
                        var properties = await contributor.GetAccessdProperties(subjectContext);
                        allowProperties.AddIfNotContains(properties);
                    }

                    if (DataProtectionOptions.Value.EntityIgnoreProperties.TryGetValue(entityType, out var entityIgnoreProps))
                    {
                        allowProperties.AddIfNotContains(entityIgnoreProps);
                    }

                    allowProperties.AddIfNotContains(DataProtectionOptions.Value.GlobalIgnoreProperties);

                    foreach (var property in entry.Properties)
                    {
                        if (!allowProperties.Contains(property.Metadata.Name))
                        {
                            property.IsModified = false;
                        }
                    }
                }
            }
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

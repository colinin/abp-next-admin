using AutoMapper;
using Elsa.Persistence.Specifications;
using Elsa.Serialization;
using Elsa.WorkflowSettings.Models;
using Elsa.WorkflowSettings.Persistence.EntityFramework.Core.Services;
using Elsa.WorkflowSettings.Persistence.EntityFramework.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.Stores;
public class FixedEntityFrameworkWorkflowSettingsStore : EntityFrameworkWorkflowSettingsStore
{
    public FixedEntityFrameworkWorkflowSettingsStore(
        IWorkflowSettingsContextFactory dbContextFactory,
        IMapper mapper,
        IContentSerializer contentSerializer,
        ILogger<FixedEntityFrameworkWorkflowSettingsStore> logger)
        : base(dbContextFactory, mapper, contentSerializer, logger)
    {
    }

    public async override Task<int> DeleteManyAsync(ISpecification<WorkflowSetting> specification, CancellationToken cancellationToken = default)
    {
        var filter = MapSpecification(specification);
        return await DoWork(async dbContext =>
        {
#if NET7_0_OR_GREATER
            return await dbContext.Set<WorkflowSetting>().Where(filter).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
#else
                var tuple = dbContext.Set<WorkflowSetting>().Where(filter).Select(x => x.Id).ToParametrizedSql();
                var entityLetter = dbContext.Set<WorkflowSetting>().EntityType.GetTableName()!.ToLowerInvariant()[0];
                var helper = dbContext.GetService<ISqlGenerationHelper>();
                var whereClause = tuple.Item1
                    .Substring(tuple.Item1.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase))
                    .Replace($"{helper.DelimitIdentifier(entityLetter.ToString())}.", string.Empty);

                for (var i = 0; i < tuple.Item2.Count(); i++)
                {
                    var sqlParameter = tuple.Item2.ElementAt(i);
                    whereClause = whereClause.Replace(sqlParameter.ParameterName,  "{" +$"{i}" + "}");
                }
                
                var parameters = tuple.Item2.Select(x => x.Value).ToArray();
                return await dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {dbContext.Set<WorkflowSetting>().EntityType.GetSchemaQualifiedTableNameWithQuotes(helper)} {whereClause}", parameters, cancellationToken);
#endif
        }, cancellationToken);
    }
}

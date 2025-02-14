using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.Specifications;
using Elsa.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.Stores;
public class FixedEntityFrameworkWorkflowDefinitionStore : EntityFrameworkWorkflowDefinitionStore
{
    public FixedEntityFrameworkWorkflowDefinitionStore(
        IElsaContextFactory dbContextFactory, 
        IMapper mapper,
        IContentSerializer contentSerializer, 
        ILogger<EntityFrameworkWorkflowDefinitionStore> logger)
        : base(dbContextFactory, mapper, contentSerializer, logger)
    {
    }

    public async override Task<int> DeleteManyAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
    {
        var filter = MapSpecification(specification);
        return await DoWork(async dbContext =>
        {
#if NET7_0_OR_GREATER
            return await dbContext.Set<WorkflowDefinition>().Where(filter).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
#else
                var tuple = dbContext.Set<WorkflowDefinition>().Where(filter).Select(x => x.Id).ToParametrizedSql();
                var entityLetter = dbContext.Set<WorkflowDefinition>().EntityType.GetTableName()!.ToLowerInvariant()[0];
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
                return await dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {dbContext.Set<WorkflowDefinition>().EntityType.GetSchemaQualifiedTableNameWithQuotes(helper)} {whereClause}", parameters, cancellationToken);
#endif
        }, cancellationToken);
    }
}

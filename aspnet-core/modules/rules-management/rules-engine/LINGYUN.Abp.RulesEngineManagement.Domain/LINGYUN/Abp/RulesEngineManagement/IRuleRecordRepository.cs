using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.RulesEngineManagement;
public interface IRuleRecordRepository : IRepository<RuleRecord, Guid>
{
    Task<List<RuleRecord>> GetChildrenRulesAsync(
        Guid ruleId, 
        CancellationToken cancellationToken = default);
    Task<List<ParamRecord>> GetParamsAsync(
        Guid ruleId, 
        CancellationToken cancellationToken = default);
    Task<ActionRecord> GetSuccessActionAsync(
        Guid ruleId, 
        CancellationToken cancellationToken = default);
    Task<ActionRecord> GetFailureActionAsync(
        Guid ruleId,
        CancellationToken cancellationToken = default);
}

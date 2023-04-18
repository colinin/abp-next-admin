using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;
public class EfCoreRuleRecordRepository : EfCoreRepository<IRulesEngineManagementDbContext, RuleRecord, Guid>, IRuleRecordRepository
{
    public EfCoreRuleRecordRepository(
        IDbContextProvider<IRulesEngineManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<RuleRecord>> GetChildrenRulesAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.ParentId == ruleId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<ActionRecord> GetFailureActionAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var actionQuery = from action in dbContext.Set<ActionRecord>()
                          join ruleAction in dbContext.Set<RuleActionRecord>()
                            on action.Id equals ruleAction.ActionId
                          where ruleAction.RuleId == ruleId && ruleAction.ActionType == ActionType.Failure
                          select action;

        return await actionQuery
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ParamRecord>> GetParamsAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var paramQuery = from param in dbContext.Set<ParamRecord>()
                          join ruleParam in dbContext.Set<RuleParamRecord>()
                              on param.Id equals ruleParam.ParamId
                          where ruleParam.RuleId == ruleId
                          select param;

        return await paramQuery
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<ActionRecord> GetSuccessActionAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var actionQuery = from action in dbContext.Set<ActionRecord>()
                          join ruleAction in dbContext.Set<RuleActionRecord>()
                            on action.Id equals ruleAction.ActionId
                          where ruleAction.RuleId == ruleId && ruleAction.ActionType == ActionType.Success
                          select action;

        return await actionQuery
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}

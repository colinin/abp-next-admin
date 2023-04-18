using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class RuleRecord : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid? ParentId { get; protected set; }
    public virtual bool Enabled { get; set; }
    public virtual string Name { get; protected set; }
    public virtual string Operator { get; protected set; }
    public virtual string ErrorMessage { get; protected set; }
    public virtual RuleExpressionType RuleExpressionType { get; set; }
    public virtual string InjectWorkflows { get; set; }
    public virtual string Expression { get; protected set; }
    public virtual string SuccessEvent { get; set; }
    public virtual ICollection<RuleParamRecord> LocalParams { get; protected set; }
    public virtual RuleActionRecord OnSuccess { get; protected set; }
    public virtual RuleActionRecord OnFailure { get; protected set; }
    protected RuleRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public RuleRecord(
        Guid id,
        string name, 
        string errorMessage, 
        string expression,
        string @operator = null,
        Guid? parentId = null,
        Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), RuleRecordConsts.MaxNameLength);
        ErrorMessage = Check.NotNullOrWhiteSpace(errorMessage, nameof(errorMessage), RuleRecordConsts.MaxErrorMessageLength);
        Expression = Check.NotNullOrWhiteSpace(expression, nameof(expression), RuleRecordConsts.MaxExpressionLength);

        Operator = Check.Length(@operator, nameof(@operator), RuleRecordConsts.MaxOperatorLength);

        ParentId = parentId;
        TenantId = tenantId;

        Enabled = true;
        RuleExpressionType = RuleExpressionType.LambdaExpression;

        LocalParams = new Collection<RuleParamRecord>();

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetSuccessAction(ActionRecord action)
    {
        OnSuccess = new RuleActionRecord(Id, action.Id, ActionType.Success, TenantId);
    }

    public void SetFailureAction(ActionRecord action)
    {
        OnFailure = new RuleActionRecord(Id, action.Id, ActionType.Failure, TenantId);
    }
}

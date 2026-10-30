using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;

public class ApprovalDelegation : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string? DelegatorProvider { get; private set; }

    public string? DelegatorType { get; private set; }

    public string? DelegatorId { get; private set; }

    public string? DelegatorDisplayName { get; private set; }

    public string? DelegateProvider { get; private set; }

    public string? DelegateType { get; private set; }

    public string? DelegateId { get; private set; }

    public string? DelegateDisplayName { get; private set; }

    public DateTimeOffset EffectiveFrom { get; private set; }

    public DateTimeOffset? EffectiveTo { get; private set; }

    public bool IsEnabled { get; private set; }

    public string? Remark { get; private set; }

    private ApprovalDelegation()
    {
    }

    public ApprovalDelegation(
        Guid id,
        ParticipantReference delegator,
        ParticipantReference @delegate,
        DateTimeOffset effectiveFrom,
        DateTimeOffset? effectiveTo = null,
        string? remark = null,
        bool isEnabled = true)
        : base(id)
    {
        SetDelegator(delegator);
        SetDelegate(@delegate);
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        Remark = Check.Length(remark, nameof(remark), ApprovalDelegationConsts.MaxRemarkLength);
        IsEnabled = isEnabled;

        CheckBusinessRules();
    }

    public void Update(
        ParticipantReference @delegate,
        DateTimeOffset effectiveFrom,
        DateTimeOffset? effectiveTo,
        bool isEnabled,
        string? remark = null)
    {
        SetDelegate(@delegate);
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        IsEnabled = isEnabled;
        Remark = Check.Length(remark, nameof(remark), ApprovalDelegationConsts.MaxRemarkLength);

        CheckBusinessRules();
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public bool IsActiveAt(DateTimeOffset now)
    {
        return IsEnabled && EffectiveFrom <= now && (!EffectiveTo.HasValue || now < EffectiveTo.Value);
    }

    public ParticipantReference? GetDelegator()
    {
        return ParticipantColumns.ToReference("", DelegatorProvider, DelegatorType, DelegatorId, DelegatorDisplayName);
    }

    public ParticipantReference? GetDelegate()
    {
        return ParticipantColumns.ToReference("", DelegateProvider, DelegateType, DelegateId, DelegateDisplayName);
    }

    public bool IsDelegator(ParticipantReference participant)
    {
        return ParticipantColumns.Matches(DelegatorProvider, DelegatorType, DelegatorId, participant);
    }

    public bool IsDelegate(ParticipantReference participant)
    {
        return ParticipantColumns.Matches(DelegateProvider, DelegateType, DelegateId, participant);
    }

    private void SetDelegator(ParticipantReference delegator)
    {
        Check.NotNull(delegator, nameof(delegator));
        DelegatorProvider = delegator.Provider;
        DelegatorType = delegator.Type.ToString();
        DelegatorId = Check.NotNullOrWhiteSpace(delegator.Id, nameof(delegator), ApprovalDelegationConsts.MaxParticipantIdLength);
        DelegatorDisplayName = delegator.DisplayName;
    }

    private void SetDelegate(ParticipantReference @delegate)
    {
        Check.NotNull(@delegate, nameof(@delegate));
        DelegateProvider = @delegate.Provider;
        DelegateType = @delegate.Type.ToString();
        DelegateId = Check.NotNullOrWhiteSpace(@delegate.Id, nameof(@delegate), ApprovalDelegationConsts.MaxParticipantIdLength);
        DelegateDisplayName = @delegate.DisplayName;
    }

    private void CheckBusinessRules()
    {
        var @delegate = GetDelegate();
        if (@delegate != null && IsDelegator(@delegate))
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Delegation.SelfDelegation);
        }
        if (EffectiveTo.HasValue && EffectiveTo.Value <= EffectiveFrom)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Delegation.InvalidPeriod);
        }
    }
}

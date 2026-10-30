using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;

/// <summary>
/// 部门责任人的分配关系，一个部门可以有多个责任人，其中最多只有一个主责任人。
/// </summary>
public class OrganizationUnitManagerAssignment : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
	public Guid? TenantId { get; private set; }

	public Guid OrganizationUnitId { get; private set; }

	public Guid UserId { get; private set; }

	public bool IsPrimary { get; private set; }

	public int Order { get; private set; }

	public string? Remark { get; private set; }

	private OrganizationUnitManagerAssignment()
	{
	}

	public OrganizationUnitManagerAssignment(
		Guid id,
		Guid organizationUnitId,
		Guid userId,
		bool isPrimary = false,
		int order = 0,
		string? remark = null)
		: base(id)
	{
		OrganizationUnitId = Check.NotDefaultOrNull<Guid>(organizationUnitId, nameof(organizationUnitId));
		UserId = Check.NotDefaultOrNull<Guid>(userId, nameof(userId));
		IsPrimary = isPrimary;
		Order = Check.Range(order, nameof(order), 0, int.MaxValue);
		Remark = Check.Length(remark, nameof(remark), OrganizationUnitManagerConsts.MaxRemarkLength);
	}

	public void SetPrimary(bool isPrimary)
	{
		IsPrimary = isPrimary;
	}

	public void ChangeOrder(int order)
	{
		Order = Check.Range(order, nameof(order), 0, int.MaxValue);
	}

	public void SetRemark(string? remark)
	{
		Remark = Check.Length(remark, nameof(remark), OrganizationUnitManagerConsts.MaxRemarkLength);
	}
}

using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore;

[ConnectionStringName(ElsaNextApprovalFlowDbProperties.ConnectionStringName)]
public interface IElsaNextApprovalFlowDbContext : IEfCoreDbContext
{
    DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }

    DbSet<ApprovalInstance> ApprovalInstances { get; set; }

    DbSet<ApprovalRecord> ApprovalRecords { get; set; }

    DbSet<ApprovalCc> ApprovalCcs { get; set; }

    DbSet<ApprovalNodeTask> ApprovalNodeTasks { get; set; }

    DbSet<ApprovalDelegation> ApprovalDelegations { get; set; }

    DbSet<FormDefinition> FormDefinitions { get; set; }

    DbSet<FormData> FormDatas { get; set; }

    DbSet<OrganizationUnitManagerAssignment> OrganizationUnitManagerAssignments { get; set; }
}

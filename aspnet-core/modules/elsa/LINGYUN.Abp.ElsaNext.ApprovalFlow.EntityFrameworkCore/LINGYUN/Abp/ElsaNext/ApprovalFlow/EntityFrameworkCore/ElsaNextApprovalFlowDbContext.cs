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
public class ElsaNextApprovalFlowDbContext : AbpDbContext<ElsaNextApprovalFlowDbContext>, IElsaNextApprovalFlowDbContext
{
    public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }

    public DbSet<ApprovalInstance> ApprovalInstances { get; set; }

    public DbSet<ApprovalRecord> ApprovalRecords { get; set; }

    public DbSet<ApprovalCc> ApprovalCcs { get; set; }

    public DbSet<ApprovalNodeTask> ApprovalNodeTasks { get; set; }

    public DbSet<ApprovalDelegation> ApprovalDelegations { get; set; }

    public DbSet<FormDefinition> FormDefinitions { get; set; }

    public DbSet<FormData> FormDatas { get; set; }

    public DbSet<OrganizationUnitManagerAssignment> OrganizationUnitManagerAssignments { get; set; }

    public ElsaNextApprovalFlowDbContext(DbContextOptions<ElsaNextApprovalFlowDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureElsaNextApprovalFlow();
    }
}

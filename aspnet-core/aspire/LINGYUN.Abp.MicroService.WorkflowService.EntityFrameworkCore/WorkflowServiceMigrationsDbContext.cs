using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.WorkflowService;

/// <summary>
/// 承载服务审批流相关模块的迁移 DbContext。
/// </summary>
/// <remarks>
/// 这里<b>只</b>配置本服务新增模块的表。Elsa 自身的表由 Elsa 的启动迁移器写入 Elsa schema，
/// 已被迁移过的其他模块若在此重复配置，会生成针对既有表的 CreateTable 而直接失败。
/// </remarks>
[ConnectionStringName("Default")]
public class WorkflowServiceMigrationsDbContext :
    AbpDbContext<WorkflowServiceMigrationsDbContext>,
    IElsaNextApprovalFlowDbContext
{
    public DbSet<OrganizationUnitManagerAssignment> OrganizationUnitManagerAssignments { get; set; }

    public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }
    public DbSet<ApprovalInstance> ApprovalInstances { get; set; }
    public DbSet<ApprovalRecord> ApprovalRecords { get; set; }
    public DbSet<ApprovalCc> ApprovalCcs { get; set; }
    public DbSet<ApprovalNodeTask> ApprovalNodeTasks { get; set; }
    public DbSet<ApprovalDelegation> ApprovalDelegations { get; set; }

    public DbSet<FormDefinition> FormDefinitions { get; set; }
    public DbSet<FormData> FormDatas { get; set; }

    public WorkflowServiceMigrationsDbContext(DbContextOptions<WorkflowServiceMigrationsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureElsaNextApprovalFlow();
    }
}

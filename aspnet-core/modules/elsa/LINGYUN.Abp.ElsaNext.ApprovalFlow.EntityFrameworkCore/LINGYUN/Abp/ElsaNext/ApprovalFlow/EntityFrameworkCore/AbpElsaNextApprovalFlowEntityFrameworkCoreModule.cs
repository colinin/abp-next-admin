using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Delegations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.OrganizationUnitManagers;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Templates;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore;

[DependsOn(
    typeof(AbpElsaNextApprovalFlowDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpElsaNextApprovalFlowEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ElsaNextApprovalFlowDbContext>(options =>
        {
            options.AddDefaultRepositories(false);

            options.AddRepository<ApprovalTemplate, EfCoreApprovalTemplateRepository>();
            options.AddRepository<ApprovalInstance, EfCoreApprovalInstanceRepository>();
            options.AddRepository<ApprovalNodeTask, EfCoreApprovalNodeTaskRepository>();
            options.AddRepository<ApprovalRecord, EfCoreApprovalRecordRepository>();
            options.AddRepository<ApprovalCc, EfCoreApprovalCcRepository>();
            options.AddRepository<ApprovalDelegation, EfCoreApprovalDelegationRepository>();
            options.AddRepository<FormDefinition, EfCoreFormDefinitionRepository>();
            options.AddRepository<FormData, EfCoreFormDataRepository>();
            options.AddRepository<OrganizationUnitManagerAssignment, EfCoreOrganizationUnitManagerRepository>();
        });}
}

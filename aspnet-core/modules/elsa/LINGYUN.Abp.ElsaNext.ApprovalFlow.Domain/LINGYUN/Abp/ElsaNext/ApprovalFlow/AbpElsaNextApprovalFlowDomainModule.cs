using Elsa.ApprovalFlow.Contracts;
using Elsa.UserTasks.Contracts;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Handlers;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(
    typeof(AbpDddDomainModule),
	typeof(AbpElsaNextApprovalFlowDomainSharedModule))]
public class AbpElsaNextApprovalFlowDomainModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddNotificationHandler<UserTaskCreatedHandler>();
		context.Services.AddNotificationHandler<UserTaskOverdueHandler>();
		context.Services.AddNotificationHandler<WorkflowFinishedHandler>();
		context.Services.AddTransient<IApprovalWorkflowStarter, ElsaApprovalWorkflowStarter>();
		context.Services.AddTransient<IApprovalActorAccessor, ApprovalActorAccessor>();
		context.Services.Replace(ServiceDescriptor.Scoped<IUserTaskAccessPolicy, ApprovalFlowUserTaskAccessPolicy>());
		context.Services.AddTransient<IApprovalCcSink, ApprovalCcStore>();
	}
}

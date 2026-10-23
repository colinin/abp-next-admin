using Elsa.UserTasks.Contracts;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using LINGYUN.Abp.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(
    typeof(AbpElsaNextApprovalFlowDomainModule),
    typeof(AbpIdentityDomainModule))]
public class AbpElsaNextApprovalFlowDomainIdentityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Identity-backed implementation of the participant directory the User Tasks module exposes:
        // replacing the Elsa default is what lets tasks target ABP users, roles and organization units.
        context.Services.AddScoped<IUserTaskParticipantDirectory, ElsaOrganizationParticipantDirectory>();

        // ABP roles become "role" groups on the actor so role-targeted approval rules match candidate groups.
        context.Services.AddSingleton<IUserTaskIdentityResolver, AbpUserTaskIdentityResolver>();
    }
}

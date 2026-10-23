using Elsa.AI.Copilot.Options;
using Elsa.AI.Host.Features;
using Elsa.AI.Host.Options;
using Elsa.AI.Host.Services;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LINGYUN.Abp.ElsaNext.AI.Features;

public class AbpAIFeature(IModule module) : FeatureBase(module)
{
    public Action<AIHostOptions>? ConfigureOptions { get; set; }
    public Action<CopilotOptions>? ConfigureCopilotOptions { get; set; }
    public Action<AIToolEnablementService>? ConfigureToolEnablement { get; set; }

    public override void Configure()
    {
        Module.AddFastEndpointsAssembly<AIFeature>();
    }

    public override void Apply()
    {
        Services.AddAIHostServices(ConfigureOptions);
        Services.AddCopilotAIProvider(ConfigureCopilotOptions);
        if (ConfigureToolEnablement != null)
        {
            Services.AddSingleton(ConfigureToolEnablement);
        }
        Module.AddFastEndpointsFromModule();
    }

    public AbpAIFeature EnableWorkflowProposalTools()
    {
        ConfigureToolEnablement += enablement =>
        {
            enablement.Enable("workflows.proposeCreate");
            enablement.Enable("workflows.proposeUpdate");
        };
        return this;
    }
}

using Elsa.ApprovalFlow.Extensions;
using Elsa.ApprovalFlow.Options;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using System;

namespace Elsa.ApprovalFlow.Features;

public sealed class ApprovalFlowFeature : FeatureBase
{
	public Action<ApprovalFlowOptions>? ConfigureOptions { get; set; }

	public ApprovalFlowFeature(IModule module)
		: base(module)
	{
	}

	public override void Configure()
	{
        Module.AddActivitiesFrom<ApprovalFlowFeature>();
	}

	public override void Apply()
	{
        Services.AddApprovalFlowServices(ConfigureOptions);
	}
}

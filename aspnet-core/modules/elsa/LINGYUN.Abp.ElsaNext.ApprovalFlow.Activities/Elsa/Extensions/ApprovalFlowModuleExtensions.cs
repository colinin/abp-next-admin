using Elsa.ApprovalFlow.Features;
using Elsa.Features.Services;
using System;

namespace Elsa.Extensions;

public static class ApprovalFlowModuleExtensions
{
	public static IModule UseApprovalFlow(this IModule module, Action<ApprovalFlowFeature>? configure = null)
	{
		return module.Use(configure);
	}
}

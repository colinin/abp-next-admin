using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Options;
using Elsa.ApprovalFlow.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Elsa.ApprovalFlow.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApprovalFlowServices(this IServiceCollection services, Action<ApprovalFlowOptions>? configure = null)
	{
		if (configure != null)
		{
			services.Configure(configure);
		}
		services.AddOptions<ApprovalFlowOptions>();
		services.TryAddEnumerable(ServiceDescriptor.Singleton<IApproverRuleResolver, BuiltInApproverRuleResolver>());
		services.TryAddSingleton<IApproverResolver, DefaultApproverResolver>();
		services.TryAddSingleton<IApprovalCompletionPolicy, DefaultApprovalCompletionPolicy>();
		services.TryAddSingleton<IApprovalCcSink, NullApprovalCcSink>();
		return services;
	}
}

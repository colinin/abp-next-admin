using Elsa.ApprovalFlow.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Services;

public sealed class NullApprovalCcSink(ILogger<NullApprovalCcSink> logger) : IApprovalCcSink
{
	public Task NotifyAsync(ApprovalCcRequest request, CancellationToken cancellationToken = default)
	{
		logger.LogDebug("The approval flow has not configured the recipients for the copy notifications. The node {NodeInstanceId} has discarded {RecipientCount} copy notification records.", request.NodeInstanceId, request.Recipients.Count);
		return Task.CompletedTask;
	}
}

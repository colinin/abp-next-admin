using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingPartitionKeyResolver :
    IOperationRateLimitingPartitionKeyResolver,
    ITransientDependency
{
    protected ICurrentUser CurrentUser { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IOperationRateLimitingClientInfoProvider ClientInfoProvider { get; }

    protected AbpOperationRateLimitingOptions Options { get; }

    public OperationRateLimitingPartitionKeyResolver(
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        IServiceProvider serviceProvider,
        IOptions<AbpOperationRateLimitingOptions> options)
    {
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        ClientInfoProvider = serviceProvider.GetService<IOperationRateLimitingClientInfoProvider>()
                             ?? NullOperationRateLimitingClientInfoProvider.Instance;
        Options = options.Value;
    }

    public virtual async Task<string> ResolveAsync(
        OperationRateLimitingRuleDefinition rule,
        OperationRateLimitingContext context,
        CancellationToken cancellationToken = default)
    {
        switch (rule.PartitionType)
        {
            case OperationRateLimitingPartitionType.Parameter:
                return context.Parameter
                    ?? throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses PartitionByParameter but no parameter value was provided.");

            case OperationRateLimitingPartitionType.CurrentUser:
                return CurrentUser.Id?.ToString()
                    ?? throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses PartitionByCurrentUser but the current user is not authenticated.");

            case OperationRateLimitingPartitionType.CurrentTenant:
                return CurrentTenant.Id?.ToString() ?? AbpOperationRateLimitingConsts.HostTenantName;

            case OperationRateLimitingPartitionType.ClientIp:
                return ClientInfoProvider.GetClientIpAddress()
                    ?? throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses PartitionByClientIp but the client IP address could not be resolved. This partition type requires an ASP.NET Core environment.");

            case OperationRateLimitingPartitionType.Email:
                var email = context.Parameter ?? CurrentUser.Email;
                return NormalizeEmail(email)
                    ?? throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses PartitionByEmail but no email value was provided or available.");

            case OperationRateLimitingPartitionType.PhoneNumber:
                var phoneNumber = context.Parameter ?? CurrentUser.PhoneNumber;
                return NormalizePhoneNumber(phoneNumber)
                    ?? throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses PartitionByPhoneNumber but no phone number was provided or available.");

            case OperationRateLimitingPartitionType.Custom:
                if (string.IsNullOrWhiteSpace(rule.PartitionResolverName))
                {
                    throw new AbpException($"The rule '{rule.Name ?? rule.Algorithm}' uses a custom partition without a resolver name.");
                }

                var resolver = Options.GetPartitionKeyResolverOrNull(rule.PartitionResolverName!);
                if (resolver == null)
                {
                    throw new AbpException($"Undefined partition key resolver: {rule.PartitionResolverName}.");
                }

                return await resolver(context) ?? string.Empty;

            default:
                throw new AbpException($"Unsupported operation rate limiting partition type: {rule.PartitionType}");
        }
    }

    public static string? NormalizeEmail(string? email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToUpperInvariant();
    }

    public static string? NormalizePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return null;
        }

        var builder = new System.Text.StringBuilder(phoneNumber.Length);

        foreach (var character in phoneNumber)
        {
            if (char.IsDigit(character) || character == '+')
            {
                builder.Append(character);
            }
        }

        return builder.Length == 0 ? null : builder.ToString();
    }
}

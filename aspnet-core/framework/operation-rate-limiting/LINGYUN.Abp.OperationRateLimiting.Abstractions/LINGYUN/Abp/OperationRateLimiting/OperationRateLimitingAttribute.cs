using System;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Enforces an operation rate limiting policy on an application service method or an MVC
/// controller action.
/// </summary>
/// <remarks>
/// The decorated service must be resolved from dependency injection and the method must be
/// interceptable (keep it <c>virtual</c> when class based proxies are used). MVC actions are
/// enforced by the action filter of the <c>LINGYUN.Abp.OperationRateLimiting.AspNetCore</c> package.
/// A method level attribute takes precedence over a class level attribute.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class OperationRateLimitingAttribute : Attribute
{
    /// <summary>
    /// Name of the policy to enforce.
    /// </summary>
    public string PolicyName { get; }

    public OperationRateLimitingAttribute(string policyName)
    {
        Check.NotNullOrWhiteSpace(policyName, nameof(policyName));

        PolicyName = policyName;
    }
}

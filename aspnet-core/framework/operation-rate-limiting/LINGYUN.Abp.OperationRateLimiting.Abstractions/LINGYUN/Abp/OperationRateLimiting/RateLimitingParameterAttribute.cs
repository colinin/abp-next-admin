using System;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Marks the method parameter whose value is used as
/// <see cref="OperationRateLimitingContext.Parameter"/>.
/// Only one parameter can be marked on a method.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class RateLimitingParameterAttribute : Attribute
{
}

using System;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Resolves <see cref="OperationRateLimitingContext.Parameter"/> from method arguments.
/// </summary>
public static class OperationRateLimitingParameterResolver
{
    /// <summary>
    /// Resolution order:
    /// <list type="number">
    /// <item>a parameter marked with <see cref="RateLimitingParameterAttribute"/>;</item>
    /// <item>a parameter whose value implements <see cref="IHasOperationRateLimitingParameter"/>;</item>
    /// <item><c>null</c>.</item>
    /// </list>
    /// </summary>
    public static string? Resolve(
        MethodInfo method,
        IReadOnlyList<object?> arguments,
        Action<string>? warn = null)
    {
        var parameters = method.GetParameters();
        var markedParameterIndexes = new List<int>();

        for (var index = 0; index < parameters.Length; index++)
        {
            if (parameters[index].IsDefined(typeof(RateLimitingParameterAttribute), inherit: true))
            {
                markedParameterIndexes.Add(index);
            }
        }

        if (markedParameterIndexes.Count > 1)
        {
            throw new AbpException(
                $"Only one parameter of '{method.DeclaringType?.FullName}.{method.Name}' can be marked with {nameof(RateLimitingParameterAttribute)}.");
        }

        if (markedParameterIndexes.Count == 1)
        {
            var index = markedParameterIndexes[0];

            return index < arguments.Count ? arguments[index]?.ToString() : null;
        }

        for (var index = 0; index < arguments.Count; index++)
        {
            if (arguments[index] is IHasOperationRateLimitingParameter partitionParameter)
            {
                return partitionParameter.GetPartitionParameter();
            }
        }

        if (parameters.Length > 0)
        {
            warn?.Invoke(
                $"No rate limiting parameter was resolved for '{method.DeclaringType?.FullName}.{method.Name}'. " +
                $"Mark one parameter with {nameof(RateLimitingParameterAttribute)} or implement {nameof(IHasOperationRateLimitingParameter)} " +
                "if the policy uses a parameter based partition.");
        }

        return null;
    }
}

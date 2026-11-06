using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Thrown when a rate limit is exceeded. It is a <see cref="BusinessException"/> with HTTP
/// status code 429 (Too Many Requests).
/// </summary>
public class AbpOperationRateLimitingException : BusinessException, IHasHttpStatusCode
{
    public int HttpStatusCode => 429;

    public string PolicyName { get; }

    public OperationRateLimitingStatus Status { get; }

    public AbpOperationRateLimitingException(
        string policyName,
        OperationRateLimitingStatus status,
        IOperationRateLimitingFormatter formatter,
        OperationRateLimitingContext? context = null,
        string? errorCode = null)
        : base(
            code: errorCode ?? (IsPermanent(status) ? AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently : AbpOperationRateLimitingErrorCodes.ExceedLimit),
            message: CreateMessage(status, formatter))
    {
        PolicyName = policyName;
        Status = status;

        Data["PolicyName"] = policyName;
        Data["MaxCount"] = status.MaxCount;
        Data["CurrentCount"] = status.CurrentCount;
        Data["RemainingCount"] = status.RemainingCount;
        Data["RetryAfterSeconds"] = (int)(status.RetryAfter?.TotalSeconds ?? 0);
        Data["RetryAfterMinutes"] = (int)(status.RetryAfter?.TotalMinutes ?? 0);
        Data["WindowDurationSeconds"] = (int)status.WindowDuration.TotalSeconds;
        Data["WindowDescription"] = formatter.FormatDuration(status.WindowDuration);

        if (status.RetryAfter.HasValue)
        {
            Data["RetryAfter"] = formatter.FormatRetryAfter(status.RetryAfter);
        }

        Data["RuleDetails"] = CreateRuleDetails(status);

        if (context != null)
        {
            foreach (var property in context.Properties)
            {
                Data[property.Key] = property.Value;
            }
        }
    }

    protected static bool IsPermanent(OperationRateLimitingStatus status)
    {
        foreach (var rule in status.Rules)
        {
            if (!rule.IsAllowed && rule.IsPermanent)
            {
                return true;
            }
        }

        return false;
    }

    protected static string CreateMessage(OperationRateLimitingStatus status, IOperationRateLimitingFormatter formatter)
    {
        if (IsPermanent(status))
        {
            return "Operation rate limit exceeded. This request is permanently denied.";
        }

        return status.RetryAfter.HasValue
            ? $"Operation rate limit exceeded. Please try again in {formatter.FormatRetryAfter(status.RetryAfter)}."
            : "Operation rate limit exceeded.";
    }

    protected static List<Dictionary<string, object?>> CreateRuleDetails(OperationRateLimitingStatus status)
    {
        var details = new List<Dictionary<string, object?>>();

        foreach (var rule in status.Rules)
        {
            details.Add(new Dictionary<string, object?>
            {
                ["RuleName"] = rule.RuleName,
                ["IsAllowed"] = rule.IsAllowed,
                ["MaxCount"] = rule.MaxCount,
                ["CurrentCount"] = rule.CurrentCount,
                ["RemainingCount"] = rule.RemainingCount,
                ["RetryAfterSeconds"] = (int)(rule.RetryAfter?.TotalSeconds ?? 0),
                ["WindowDurationSeconds"] = (int)rule.WindowDuration.TotalSeconds,
                ["IsPermanent"] = rule.IsPermanent
            });
        }

        return details;
    }
}

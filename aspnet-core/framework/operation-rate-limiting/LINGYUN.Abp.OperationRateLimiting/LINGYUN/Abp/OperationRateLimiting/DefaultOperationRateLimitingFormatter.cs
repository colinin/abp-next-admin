using System;
using LINGYUN.Abp.OperationRateLimiting.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default formatter. Replace <see cref="IOperationRateLimitingFormatter"/> to customize
/// how durations are displayed.
/// </summary>
public class DefaultOperationRateLimitingFormatter : IOperationRateLimitingFormatter, ITransientDependency
{
    protected IStringLocalizer<OperationRateLimitingResource> Localizer { get; }

    public DefaultOperationRateLimitingFormatter(IStringLocalizer<OperationRateLimitingResource> localizer)
    {
        Localizer = localizer;
    }

    public virtual string FormatDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            return Localizer["DurationFormat.Seconds", 0];
        }

        if (duration.TotalDays >= 1)
        {
            var days = (int)duration.TotalDays;
            var hours = duration.Hours;

            return Localizer["DurationFormat.Days", days, hours];
        }

        if (duration.TotalHours >= 1)
        {
            var hours = (int)duration.TotalHours;
            var minutes = duration.Minutes;

            return Localizer["DurationFormat.Hours", hours, minutes];
        }

        if (duration.TotalMinutes >= 1)
        {
            var minutes = (int)duration.TotalMinutes;
            var seconds = duration.Seconds;

            return Localizer["DurationFormat.Minutes", minutes, seconds];
        }

        if (duration.TotalSeconds >= 1)
        {
            return Localizer["DurationFormat.Seconds", (int)Math.Ceiling(duration.TotalSeconds)];
        }

        return Localizer["DurationFormat.Milliseconds", (int)Math.Ceiling(duration.TotalMilliseconds)];
    }

    public virtual string FormatRetryAfter(TimeSpan? retryAfter)
    {
        return retryAfter.HasValue ? FormatDuration(retryAfter.Value) : string.Empty;
    }
}

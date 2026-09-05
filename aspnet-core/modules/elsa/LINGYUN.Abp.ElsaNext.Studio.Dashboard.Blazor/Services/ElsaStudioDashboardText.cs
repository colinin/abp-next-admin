using Elsa.Studio.Dashboard.Models;
using Elsa.Studio.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Translations;
using System.Globalization;

namespace LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Services;

/// <summary>
/// Culture-aware text and metric formatting for the dashboard widgets. The official Elsa Studio
/// dashboard widgets hard-code their English copy and format numbers with the ambient thread
/// culture, so this module renders localized copies that resolve every label through
/// <see cref="ILocalizer"/> (which follows the ABP UI culture captured on the circuit) and format
/// numbers with that same culture.
/// </summary>
public class ElsaStudioDashboardText
{
    private readonly ILocalizer _localizer;

    public ElsaStudioDashboardText(ILocalizer localizer)
    {
        _localizer = localizer;
    }

    /// <summary>The UI culture captured at prerender (fallback: current thread culture).</summary>
    public CultureInfo Culture => CultureInfo.CurrentUICulture;

    /// <summary>Localized text for the given key (falls back to the key itself).</summary>
    public string this[string key] => _localizer[key];

    /// <summary>Localized capability label of the "needs attention" backend capability.</summary>
    public string CapabilityLabel(DashboardCapabilityStatus? capability)
    {
        return capability?.Status switch
        {
            "Available" => _localizer["Available"],
            "Unauthorized" => _localizer["No access"],
            "NotInstalled" => _localizer["Not installed"],
            _ => _localizer["Unavailable"]
        };
    }

    /// <summary>Localized label of the runtime status chip shown in the dashboard header.</summary>
    public string RuntimeStatusLabel(string? status)
    {
        return status switch
        {
            DashboardRuntimeStatusKeys.AcceptingWork => _localizer["Accepting work"],
            DashboardRuntimeStatusKeys.Paused => _localizer["Paused"],
            DashboardRuntimeStatusKeys.Draining => _localizer["Draining"],
            _ => _localizer["Unavailable"]
        };
    }

    /// <summary>Humanized caption for the selected dashboard range.</summary>
    public string RangeCaption(string selectedRange)
    {
        return selectedRange switch
        {
            DashboardRangeKeys.OneHour => _localizer["Last hour"],
            DashboardRangeKeys.SevenDays => _localizer["Last 7 days"],
            _ => _localizer["Last 24 hours"]
        };
    }

    public string Count(long value) => value.ToString("N0", Culture);

    public string Count(int value) => Count((long)value);

    public string Duration(TimeSpan? value)
    {
        if (value == null)
            return "N/A";

        var duration = value.Value;

        if (duration.TotalMilliseconds < 1000)
            return $"{duration.TotalMilliseconds:N0} ms";

        if (duration.TotalMinutes < 1)
            return $"{duration.TotalSeconds:N1} s";

        if (duration.TotalHours < 1)
            return $"{duration.TotalMinutes:N1} min";

        return $"{duration.TotalHours:N1} h";
    }

    public string DateTime(DateTimeOffset? value)
    {
        return value == null ? "N/A" : value.Value.ToLocalTime().ToString("g", Culture);
    }

    public string RelativeTimestamp(DateTimeOffset? value)
    {
        if (value == null)
            return "N/A";

        var elapsed = DateTimeOffset.UtcNow - value.Value.ToUniversalTime();

        if (elapsed.TotalSeconds < 60)
            return _localizer["Just now"];

        if (elapsed.TotalMinutes < 60)
            return $"{elapsed.TotalMinutes:N0} {_localizer["min ago"]}";

        if (elapsed.TotalHours < 24)
            return $"{elapsed.TotalHours:N0} {_localizer["h ago"]}";

        return $"{elapsed.TotalDays:N0} {_localizer["d ago"]}";
    }

    /// <summary>Trend chart x-axis label for a bucket timestamp.</summary>
    public string TrendLabel(DateTimeOffset value, string granularity)
    {
        var local = value.ToLocalTime();
        return granularity == DashboardTrendGranularity.Day
            ? local.ToString(Culture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) ? "M月d日" : "MMM d", Culture)
            : local.ToString("HH:mm", Culture);
    }
}

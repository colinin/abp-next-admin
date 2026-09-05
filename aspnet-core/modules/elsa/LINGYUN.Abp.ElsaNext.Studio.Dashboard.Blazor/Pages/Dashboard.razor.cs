using Elsa.Studio.Contracts;
using Elsa.Studio.Dashboard.Models;
using Elsa.Studio.Dashboard.Services;
using Elsa.Studio.Dashboard.Widgets;
using Elsa.Studio.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Pages;

/// <summary>
/// Elsa Studio dashboard page (official Elsa.Studio.Dashboard content hosted under
/// <c>/elsa/workflows/dashboard</c>). The dashboard service reads the Elsa dashboard API with the
/// circuit session token; the widgets (metrics/trend/activity/...) come from the official
/// dashboard modules.
/// </summary>
public partial class Dashboard : IAsyncDisposable
{
    private CancellationTokenSource? _loadCancellationTokenSource;
    private DashboardSnapshot? _snapshot;
    private DashboardLoadStatus _status = DashboardLoadStatus.Unavailable;
    private string _selectedRange = DashboardRangeKeys.TwentyFourHours;
    private string? _message;
    private bool _loading;
    private bool _disposed;
    private bool _subscribedToFeatureInitialized;
    private DateTimeOffset? _lastRefreshedAt;

    [Inject] private IDashboardService DashboardService { get; set; } = null!;
    [Inject] private IDashboardWidgetRegistry WidgetRegistry { get; set; } = null!;
    [Inject] private IEnumerable<DashboardWidgetDescriptor> Widgets { get; set; } = [];
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IFeatureService FeatureService { get; set; } = null!;
    [Inject] private ElsaStudioDashboardText Text { get; set; } = null!;

    private DashboardWidgetContext WidgetContext => new(
        _selectedRange,
        _loading,
        _lastRefreshedAt,
        _status,
        _message,
        _snapshot,
        RefreshAsync,
        NavigationManager);

    private string BackendLabel
    {
        get
        {
            if (_snapshot == null)
                return Localizer["Selected backend"];

            var overview = _snapshot.Overview;
            var backendName = string.IsNullOrWhiteSpace(overview.BackendName) ? Localizer["Backend"] : overview.BackendName;
            return string.IsNullOrWhiteSpace(overview.EnvironmentName) ? backendName : $"{backendName} / {overview.EnvironmentName}";
        }
    }

    private string LastRefreshedLabel => _lastRefreshedAt == null ? Localizer["Not refreshed yet"] : $"{Localizer["Refreshed"]} {Text.RelativeTimestamp(_lastRefreshedAt)}";

    private string StatusLabel => _status switch
    {
        DashboardLoadStatus.Unauthorized => Localizer["No access"],
        DashboardLoadStatus.BackendDisconnected => Localizer["Backend disconnected"],
        DashboardLoadStatus.Failed => Localizer["Refresh failed"],
        DashboardLoadStatus.Loaded => Localizer["Loaded"],
        _ => Localizer["Dashboard unavailable"]
    };

    private Severity AlertSeverity => _status switch
    {
        DashboardLoadStatus.Unauthorized => Severity.Warning,
        DashboardLoadStatus.Loaded => Severity.Info,
        _ => Severity.Error
    };

    private IReadOnlyCollection<DashboardWidgetDescriptor> GetWidgets(string zone) =>
        Widgets
            .Concat(WidgetRegistry.List())
            .DistinctBy(x => x.Id)
            .Where(x => x.Zone == zone && x.IsVisible(WidgetContext))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Id, StringComparer.Ordinal)
            .ToList();

    protected override async Task OnInitializedAsync()
    {
        // Features register their widgets into the (singleton) widget registry during
        // InitializeFeaturesAsync; the official host triggers this from its own startup tasks, so
        // do it here to be safe when hosting the page standalone in an ABP layout.
        try
        {
            await FeatureService.InitializeFeaturesAsync();
        }
        catch (Exception)
        {
            // Remote feature discovery is unavailable (e.g. backend feature API down); the page
            // still renders the DI-registered widgets.
        }
        await RefreshAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender || _disposed)
            return;

        FeatureService.Initialized += OnFeatureServiceInitialized;
        _subscribedToFeatureInitialized = true;
    }

    private void OnFeatureServiceInitialized()
    {
        _ = RefreshWidgetsAfterFeatureInitializationAsync();
    }

    private async Task RefreshWidgetsAfterFeatureInitializationAsync()
    {
        if (_disposed)
            return;

        try
        {
            await InvokeAsync(StateHasChanged);
        }
        catch (InvalidOperationException) when (_disposed)
        {
        }
    }

    private async Task OnRangeChangedAsync(string? range)
    {
        var selectedRange = DashboardRangeMapper.Normalize(range);

        if (_selectedRange == selectedRange)
            return;

        _selectedRange = selectedRange;
        await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        await LoadAsync(_selectedRange);
    }

    private async Task LoadAsync(string range)
    {
        await CancelCurrentLoadAsync();

        var cancellationTokenSource = new CancellationTokenSource();
        _loadCancellationTokenSource = cancellationTokenSource;
        _loading = true;
        _message = null;

        try
        {
            var result = await DashboardService.LoadAsync(range, cancellationToken: cancellationTokenSource.Token);
            _status = result.Status;

            if (result.Snapshot != null)
            {
                _snapshot = result.Snapshot;
                _lastRefreshedAt = DateTimeOffset.UtcNow;
                _message = null;
            }
            else
            {
                _message = result.Message;
            }
        }
        catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            _status = DashboardLoadStatus.Failed;
            _message = e.Message;
        }
        finally
        {
            if (ReferenceEquals(_loadCancellationTokenSource, cancellationTokenSource))
            {
                _loading = false;
                _loadCancellationTokenSource = null;
            }

            cancellationTokenSource.Dispose();
        }
    }

    private async Task CancelCurrentLoadAsync()
    {
        if (_loadCancellationTokenSource == null)
            return;

        await _loadCancellationTokenSource.CancelAsync();
        _loadCancellationTokenSource = null;
    }

    public async ValueTask DisposeAsync()
    {
        _disposed = true;

        if (_subscribedToFeatureInitialized)
            FeatureService.Initialized -= OnFeatureServiceInitialized;

        await CancelCurrentLoadAsync();
    }
}

using MudBlazor;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Components;

public sealed class MudTextFieldExIcon
{
    public string Icon { get; init; } = string.Empty;

    public Func<Task>? OnClick { get; init; }

    public string? Tooltip { get; init; }

    public string? AriaLabel { get; init; }

    public bool Visible { get; init; } = true;

    public bool Disabled { get; init; }

    public Color Color { get; init; } = Color.Default;
}

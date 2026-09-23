namespace LINGYUN.Platform.Menus;
public class StandardMenu
{
    public string? Icon { get; set; }
    public string Path { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Description { get; set; }
    public string? Redirect { get; set; }
}

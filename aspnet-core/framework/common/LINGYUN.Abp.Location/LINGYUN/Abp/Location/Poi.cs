namespace LINGYUN.Abp.Location;

public class Poi
{
    public string? Tag { get; set; }
    public string Name { get; set; } = default!;
    public string? Type { get; set; }
    public string Address { get; set; } = default!;
    public int? Distance { get; set; }
}

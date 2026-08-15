namespace LINGYUN.Abp.SettingManagement;

public class OptionDto
{
    public string Name { get; set; } = default!;
    public string? Value { get; set; }

    public OptionDto()
    {

    }

    public OptionDto(string name, string? value)
    {
        Name = name;
        Value = value;
    }
}

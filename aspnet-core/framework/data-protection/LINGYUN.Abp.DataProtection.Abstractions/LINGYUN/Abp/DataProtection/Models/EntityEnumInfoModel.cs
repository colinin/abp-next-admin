namespace LINGYUN.Abp.DataProtection.Models;

public class EntityEnumInfoModel
{
    public string Key { get; set; } = default!;
    public object? Value { get; set; }
    public EntityEnumInfoModel()
    {

    }
    public EntityEnumInfoModel(string key, object? value)
    {
        Key = key;
        Value = value;
    }
}

namespace LINGYUN.Abp.DataProtectionManagement;
public static class EntityPropertyInfoConsts
{
    public static int MaxNameLength { get; set; } = EntityTypeInfoConsts.MaxNameLength;
    public static int MaxDisplayNameLength { get; set; } = EntityTypeInfoConsts.MaxDisplayNameLength;
    public static int MaxTypeFullNameLength { get; set; } = EntityTypeInfoConsts.MaxTypeFullNameLength;
    public static int MaxValueRangeLength { get; set; } = 512;
}

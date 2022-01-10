namespace LINGYUN.Abp.TaskManagement;

public static class BackgroundJobInfoConsts
{
    public static int MaxCronLength { get; set; } = 50;
    public static int MaxNameLength { get; set; } = 100;
    public static int MaxGroupLength { get; set; } = 50;
    public static int MaxTypeLength { get; set; } = 1000;
    public static int MaxDescriptionLength { get; set; } = 255;
    public static int MaxResultLength { get; set; } = 1000;
}

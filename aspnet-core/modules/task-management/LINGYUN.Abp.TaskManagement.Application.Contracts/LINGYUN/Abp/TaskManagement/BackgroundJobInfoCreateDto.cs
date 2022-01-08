namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobInfoCreateDto : BackgroundJobInfoCreateOrUpdateDto
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 任务分组
    /// </summary>
    public string Group { get; set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    public string Type { get; set; }
}

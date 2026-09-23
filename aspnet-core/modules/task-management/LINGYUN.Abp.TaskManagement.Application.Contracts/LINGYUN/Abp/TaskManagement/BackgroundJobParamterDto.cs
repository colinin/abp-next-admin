namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobParamterDto
{
    public string Name { get; set; } = default!;

    public bool Required { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }
}

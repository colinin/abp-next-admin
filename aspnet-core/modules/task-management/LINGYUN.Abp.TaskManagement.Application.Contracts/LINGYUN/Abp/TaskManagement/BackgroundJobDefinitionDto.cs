using System.Collections.Generic;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobDefinitionDto
{
    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string? Description { get; set; }

    public List<BackgroundJobParamterDto> Paramters { get; set; } = new List<BackgroundJobParamterDto>();
}

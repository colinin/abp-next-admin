using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobLogDto : EntityDto<long>
{
    public string JobName { get; set; }
    public string JobGroup { get; set; }
    public string JobType { get; set; }
    public string Message { get; set; }
    public DateTime RunTime { get; set; }
    public string Exception { get; set; }
}

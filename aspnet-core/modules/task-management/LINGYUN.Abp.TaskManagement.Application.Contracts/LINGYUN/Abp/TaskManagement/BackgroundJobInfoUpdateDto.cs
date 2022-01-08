using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobInfoUpdateDto : BackgroundJobInfoCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}

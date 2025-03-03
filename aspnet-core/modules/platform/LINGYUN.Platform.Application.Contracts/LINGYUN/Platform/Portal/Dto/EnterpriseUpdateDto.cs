using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Portal;

public class EnterpriseUpdateDto : EnterpriseCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}

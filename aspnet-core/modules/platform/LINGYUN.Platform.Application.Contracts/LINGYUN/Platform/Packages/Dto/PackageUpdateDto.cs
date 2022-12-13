using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Packages;

public class PackageUpdateDto : PackageCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}

using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    [ConnectionStringName(LocalizationDbProperties.ConnectionStringName)]
    public interface ILocalizationDbContext : IEfCoreDbContext
    {
    }
}

using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    public class AbpDataProtectionDbContext<TDbContext> : AbpDbContext<TDbContext>
        where TDbContext : DbContext
    {
        public AbpDataProtectionDbContext(
            DbContextOptions<TDbContext> options) : base(options)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TextTemplating.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpTextTemplatingDbProperties.ConnectionStringName)]
public interface ITextTemplatingDbContext : IEfCoreDbContext
{
    DbSet<TextTemplate> TextTemplates { get; }
}

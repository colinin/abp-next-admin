using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TextTemplating.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpTextTemplatingDbProperties.ConnectionStringName)]
public class TextTemplatingDbContext : AbpDbContext<TextTemplatingDbContext>, ITextTemplatingDbContext
{
    public DbSet<TextTemplate> TextTemplates { get; set; }

    public DbSet<TextTemplateDefinition> TextTemplateDefinitions { get; set; }

    public TextTemplatingDbContext(DbContextOptions<TextTemplatingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTextTemplating();
    }
}

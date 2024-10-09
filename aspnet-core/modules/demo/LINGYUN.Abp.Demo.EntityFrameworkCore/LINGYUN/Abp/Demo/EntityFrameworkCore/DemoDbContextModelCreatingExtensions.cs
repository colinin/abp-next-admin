using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.Demo.EntityFrameworkCore;
public static class DemoDbContextModelCreatingExtensions
{
    public static void ConfigureDemo(
        this ModelBuilder builder,
        Action<DemoModelBuilderConfigurationOptions>? optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new DemoModelBuilderConfigurationOptions(
            DemoDbProterties.DbTablePrefix,
            DemoDbProterties.DbSchema
        );
        optionsAction?.Invoke(options);

        builder.Entity<Book>(b =>
        {
            b.ToTable(options.TablePrefix + "Books", options.Schema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();
        });

        builder.Entity<Author>(b =>
        {
            b.ToTable(options.TablePrefix + "Authors", options.Schema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AuthorConsts.MaxNameLength);

            b.HasIndex(x => x.Name);
        });
    }
}

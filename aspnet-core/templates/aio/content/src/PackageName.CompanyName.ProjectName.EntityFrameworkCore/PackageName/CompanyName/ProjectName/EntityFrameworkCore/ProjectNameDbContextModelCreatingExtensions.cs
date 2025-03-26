using Microsoft.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.Users;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public static class ProjectNameDbContextModelCreatingExtensions
{
    public static void ConfigureProjectName(
        this ModelBuilder builder,
        Action<ProjectNameModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new ProjectNameModelBuilderConfigurationOptions(
            ProjectNameDbProperties.DbTablePrefix,
            ProjectNameDbProperties.DbSchema
        );
        optionsAction?.Invoke(options);
        
        builder.Entity<User>(b =>
        {
            b.ToTable(ProjectNameDbProperties.DbTablePrefix + "Users", ProjectNameDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.NickName).HasComment("用户名称");
            b.Property(x => x.IdentityUserId).HasComment("Identity用户Id");

            // 用户与IdentityUser的关系（一对一）
            b.HasOne(x => x.IdentityUser)
                .WithOne()
                .HasForeignKey<User>(u => u.IdentityUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

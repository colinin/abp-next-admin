using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[ConnectionStringName(ProjectNameDbProperties.ConnectionStringName)]
public class ProjectNameDbContext : AbpDataProtectionDbContext<ProjectNameDbContext>, IProjectNameDbContext
{
    public ProjectNameDbContext(
        DbContextOptions<ProjectNameDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureProjectName();
    }
}

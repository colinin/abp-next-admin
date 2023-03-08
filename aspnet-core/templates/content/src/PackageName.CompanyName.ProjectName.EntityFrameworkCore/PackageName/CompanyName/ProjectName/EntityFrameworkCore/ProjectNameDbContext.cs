using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[ConnectionStringName(ProjectNameDbProperties.ConnectionStringName)]
public class ProjectNameDbContext : AbpDbContext<ProjectNameDbContext>, IProjectNameDbContext
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

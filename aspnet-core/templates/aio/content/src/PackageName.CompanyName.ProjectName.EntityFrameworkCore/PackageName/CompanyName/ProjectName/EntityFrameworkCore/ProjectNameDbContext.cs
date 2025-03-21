using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.Users;
using Volo.Abp.Data;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[ConnectionStringName(ProjectNameDbProperties.ConnectionStringName)]
public class ProjectNameDbContext : AbpDataProtectionDbContext<ProjectNameDbContext>, IProjectNameDbContext
{
    public virtual DbSet<User> Users { get; set; }
    
    public ProjectNameDbContext(
        DbContextOptions<ProjectNameDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ConfigureProjectName();
        modelBuilder.ConfigureIdentity();
        base.OnModelCreating(modelBuilder);
    }
}

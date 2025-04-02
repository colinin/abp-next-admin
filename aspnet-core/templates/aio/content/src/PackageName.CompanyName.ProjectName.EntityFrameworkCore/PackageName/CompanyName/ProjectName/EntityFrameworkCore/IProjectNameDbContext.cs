using Microsoft.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[ConnectionStringName(ProjectNameDbProperties.ConnectionStringName)]
public interface IProjectNameDbContext : IEfCoreDbContext
{
    DbSet<User> Users { get; }
}

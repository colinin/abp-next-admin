using Microsoft.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PackageName.CompanyName.ProjectName.Users
{
    public class UserRepository : EfCoreRepository<ProjectNameDbContext, User, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<ProjectNameDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<User>> WithDetailsAsync()
        {
            return (await GetDbSetAsync()).Include(x => x.IdentityUser);
        }
    }
}
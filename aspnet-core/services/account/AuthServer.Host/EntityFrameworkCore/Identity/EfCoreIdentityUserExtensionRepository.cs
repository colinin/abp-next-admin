using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace AuthServer.Host.EntityFrameworkCore.Identity
{
    public class EfCoreIdentityUserRepository : EfCoreRepository<IdentityDbContext, IdentityUser, Guid>, LINGYUN.Abp.Account.IIdentityUserRepository,
        ITransientDependency
    {
        public EfCoreIdentityUserRepository(
            IDbContextProvider<IdentityDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<IdentityUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await WithDetails()
                .Where(usr => usr.PhoneNumber.Equals(phoneNumber))
                .FirstOrDefaultAsync();
        }

        public override IQueryable<IdentityUser> WithDetails()
        {
            return DbSet
                .Include(x => x.Claims)
                .Include(x => x.Roles)
                .Include(x => x.Logins)
                .Include(x => x.Tokens);
        }
    }
}

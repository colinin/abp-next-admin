using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    public interface IIdentityUserRepository : IReadOnlyRepository<IdentityUser, Guid>
    {
        Task<bool> PhoneNumberHasRegistedAsync(string phoneNumber);

        Task<IdentityUser> FindByPhoneNumberAsync(string phoneNumber);
    }
}

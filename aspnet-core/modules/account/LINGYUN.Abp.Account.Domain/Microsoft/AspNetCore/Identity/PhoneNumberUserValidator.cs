using LINGYUN.Abp.Account.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using IIdentityUserRepository = LINGYUN.Abp.Account.IIdentityUserRepository;

namespace Microsoft.AspNetCore.Identity
{
    [Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
    [ExposeServices(typeof(IUserValidator<IdentityUser>))]
    public class PhoneNumberUserValidator : UserValidator<IdentityUser>
    {
        private readonly IStringLocalizer<AccountResource> _stringLocalizer;
        private readonly IIdentityUserRepository _identityUserRepository;

        public PhoneNumberUserValidator(
            IStringLocalizer<AccountResource> stringLocalizer,
            IIdentityUserRepository identityUserRepository)
        {
            _stringLocalizer = stringLocalizer;
            _identityUserRepository = identityUserRepository;
        }
        public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            await ValidatePhoneNumberAsync(manager, user);
            return await base.ValidateAsync(manager, user);
        }

        protected virtual async Task ValidatePhoneNumberAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var phoneNumber = await manager.GetPhoneNumberAsync(user);
            if (phoneNumber.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException(_stringLocalizer["InvalidPhoneNumber"].Value, "InvalidPhoneNumber");
            }

            var phoneNumberHasRegisted = await _identityUserRepository.PhoneNumberHasRegistedAsync(phoneNumber);
            if (phoneNumberHasRegisted)
            {
                throw new UserFriendlyException(_stringLocalizer["DuplicatePhoneNumber", phoneNumber].Value, "DuplicatePhoneNumber");
            }
        }
    }
}

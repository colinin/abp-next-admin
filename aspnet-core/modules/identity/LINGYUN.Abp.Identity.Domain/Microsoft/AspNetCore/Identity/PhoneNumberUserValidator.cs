using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace Microsoft.AspNetCore.Identity
{
    [Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
    [ExposeServices(typeof(IUserValidator<IdentityUser>))]
    public class PhoneNumberUserValidator : UserValidator<IdentityUser>
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IIdentityUserRepository _userRepository;

        public PhoneNumberUserValidator(
            IIdentityUserRepository userRepository,
            IStringLocalizer<IdentityResource> stringLocalizer)
        {
            _userRepository = userRepository;
            _stringLocalizer = stringLocalizer;
        }
        public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var errors = new List<IdentityError>();
            await ValidatePhoneNumberAsync(manager, user, errors);

            return (errors.Count > 0) 
                ? IdentityResult.Failed(errors.ToArray()) 
                : await base.ValidateAsync(manager, user);
        }

        protected async virtual Task ValidatePhoneNumberAsync(UserManager<IdentityUser> manager, IdentityUser user, ICollection<IdentityError> errors)
        {
            var phoneNumber = await manager.GetPhoneNumberAsync(user);
            if (phoneNumber.IsNullOrWhiteSpace())
            {
                return;
            }
            var findUser = await _userRepository.FindByPhoneNumberAsync(phoneNumber, false);
            if (findUser != null && !findUser.Id.Equals(user.Id))
            {
                //errors.Add(new IdentityError
                //{
                //    Code = "DuplicatePhoneNumber",
                //    Description = _stringLocalizer["DuplicatePhoneNumber", phoneNumber]
                //});
                throw new UserFriendlyException(_stringLocalizer["Volo.Abp.Identity:DuplicatePhoneNumber", phoneNumber]);
            }
        }
    }
}

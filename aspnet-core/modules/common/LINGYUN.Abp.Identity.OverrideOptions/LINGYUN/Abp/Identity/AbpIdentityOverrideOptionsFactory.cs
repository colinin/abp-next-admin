using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.Options;

namespace LINGYUN.Abp.Identity
{
    public class AbpIdentityOverrideOptionsFactory : AbpOptionsFactory<IdentityOptions>
    {
        public AbpIdentityOverrideOptionsFactory(
            IEnumerable<IConfigureOptions<IdentityOptions>> setups,
            IEnumerable<IPostConfigureOptions<IdentityOptions>> postConfigures)
            : base(setups, postConfigures)
        {

        }

        public override IdentityOptions Create(string name)
        {
            return base.Create(name);
        }
    }
}

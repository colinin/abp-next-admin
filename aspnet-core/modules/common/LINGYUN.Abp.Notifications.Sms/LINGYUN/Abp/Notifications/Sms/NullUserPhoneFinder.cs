using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications.Sms
{
    public class NullUserPhoneFinder : IUserPhoneFinder, ISingletonDependency
    {
        public Task<IEnumerable<string>> FindByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellation = default)
        {
            IEnumerable<string> emptyPhoneList = Array.Empty<string>();

            return Task.FromResult(emptyPhoneList);
        }
    }
}

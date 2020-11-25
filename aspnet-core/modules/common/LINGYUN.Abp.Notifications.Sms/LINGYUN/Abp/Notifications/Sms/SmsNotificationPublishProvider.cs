using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Sms
{
    public class SmsNotificationPublishProvider : NotificationPublishProvider
    {
        public const string ProviderName = "Sms";

        private IUserPhoneFinder _userPhoneFinder;
        protected IUserPhoneFinder UserPhoneFinder => LazyGetRequiredService(ref _userPhoneFinder);

        protected ISmsNotificationSender Sender { get; }

        protected AbpNotificationsSmsOptions Options { get; }

        public SmsNotificationPublishProvider(
            IServiceProvider serviceProvider,
            ISmsNotificationSender sender,
            IOptions<AbpNotificationsSmsOptions> options) 
            : base(serviceProvider)
        {
            Sender = sender;
            Options = options.Value;
        }

        public override string Name => ProviderName;

        protected override async Task PublishAsync(
            NotificationInfo notification,
            IEnumerable<UserIdentifier> identifiers,
            CancellationToken cancellationToken = default)
        {
            if (!identifiers.Any())
            {
                return;
            }

            var sendToPhones = await UserPhoneFinder.FindByUserIdsAsync(identifiers.Select(usr => usr.UserId), cancellationToken);
            if (!sendToPhones.Any())
            {
                return;
            }
            await Sender.SendAsync(notification, sendToPhones.JoinAsString(","));
        }
    }
}

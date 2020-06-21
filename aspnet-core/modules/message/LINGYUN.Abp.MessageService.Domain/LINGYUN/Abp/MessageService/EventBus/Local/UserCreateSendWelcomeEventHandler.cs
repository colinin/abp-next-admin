using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.EventBus
{
    public class UserCreateSendWelcomeEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IStringLocalizer _stringLocalizer;

        private readonly INotificationDispatcher _notificationDispatcher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        // 需要模拟用户令牌
        // 是否有必要
        // private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        public UserCreateSendWelcomeEventHandler(
            ISettingProvider settingProvider,
            INotificationDispatcher notificationDispatcher,
            IStringLocalizer<MessageServiceResource> stringLocalizer,
            INotificationSubscriptionManager notificationSubscriptionManager
            //ICurrentPrincipalAccessor currentPrincipalAccessor
            )
        {
            _settingProvider = settingProvider;
            _stringLocalizer = stringLocalizer;

            _notificationDispatcher = notificationDispatcher;
            _notificationSubscriptionManager = notificationSubscriptionManager;

            //_currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            // 获取默认语言
            var userDefaultCultureName = await _settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            if (userDefaultCultureName.IsNullOrWhiteSpace())
            {
                userDefaultCultureName = CultureInfo.CurrentUICulture.Name;
                // CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(userDefaultCultureName);
            }
            using (CultureHelper.Use(userDefaultCultureName, userDefaultCultureName))
            {
                var userIdentifer = new UserIdentifier(eventData.Entity.Id, eventData.Entity.UserName);
                // 订阅用户欢迎消息
                await _notificationSubscriptionManager.SubscribeAsync(eventData.Entity.TenantId,
                    userIdentifer, UserNotificationNames.WelcomeToApplication);

                // Store未检查已订阅
                //await _notificationStore.InsertUserSubscriptionAsync(eventData.Entity.TenantId,
                //    userIdentifer, UserNotificationNames.WelcomeToApplication);

                var userWelcomeNotifictionData = new NotificationData();

                userWelcomeNotifictionData.WriteStandardData(
                    L("WelcomeToApplicationFormUser", eventData.Entity.Name ?? eventData.Entity.UserName),
                    L("WelcomeToApplicationFormUser", eventData.Entity.Name ?? eventData.Entity.UserName),
                    DateTime.Now, eventData.Entity.UserName);

                // 换成用户名称,而不是用户名
                // userWelcomeNotifictionData.Properties["message"] = L("WelcomeToApplicationFormUser", eventData.Entity.Name);

                var noticeNormalizerName = NotificationNameNormalizer.NormalizerName(UserNotificationNames.WelcomeToApplication);
                await _notificationDispatcher.DispatchAsync(noticeNormalizerName, userWelcomeNotifictionData, eventData.Entity.TenantId);
            }
        }

        //public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        //{
        //    // 模拟用户令牌
        //    var mockUserPrincipal = new ClaimsPrincipal();
        //    var mockUserIdentity = new ClaimsIdentity();
        //    mockUserIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, eventData.Entity.Id.ToString()));
        //    mockUserIdentity.AddClaim(new Claim(AbpClaimTypes.UserName, eventData.Entity.UserName));
        //    mockUserIdentity.AddClaim(new Claim(AbpClaimTypes.Email, eventData.Entity.Email));
        //    mockUserIdentity.AddClaim(new Claim(AbpClaimTypes.PhoneNumber, eventData.Entity.PhoneNumber));
        //    if (eventData.Entity.TenantId.HasValue)
        //    {
        //        mockUserIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, eventData.Entity.TenantId.ToString()));
        //    }

        //    mockUserPrincipal.AddIdentity(mockUserIdentity);
        //    using (_currentPrincipalAccessor.Change(mockUserPrincipal))
        //    {
        //        // 获取默认语言
        //        // TODO: 是否采用系统默认语言而不是用户默认语言?
        //        var userDefaultCultureName = await _settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
        //        if (!userDefaultCultureName.IsNullOrWhiteSpace())
        //        {
        //            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(userDefaultCultureName);
        //        }
        //        // 订阅用户欢迎消息
        //        await _notificationStore.InsertUserSubscriptionAsync(eventData.Entity.TenantId,
        //            eventData.Entity.Id, UserNotificationNames.WelcomeToApplication);

        //        var userWelcomeNotifiction = new NotificationInfo
        //        {
        //            CreationTime = DateTime.Now,
        //            Name = UserNotificationNames.WelcomeToApplication,
        //            NotificationSeverity = NotificationSeverity.Info,
        //            NotificationType = NotificationType.System,
        //            TenantId = eventData.Entity.TenantId
        //        };
        //        userWelcomeNotifiction.Data.Properties["message"] = L("WelcomeToApplicationFormUser", eventData.Entity.UserName);

        //        await _notificationDispatcher.DispatcheAsync(userWelcomeNotifiction);
        //    }
        //}

        protected string L(string name, params object[] args)
        {
            return _stringLocalizer[name, args]?.Value;
        }
    }
}

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
using Volo.Abp.Users.Notifications;

namespace LINGYUN.Abp.MessageService.EventBus
{
    public class UserCreateSendWelcomeEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly INotificationStore _notificationStore;
        private readonly INotificationDispatcher _notificationDispatcher;

        // 需要模拟用户令牌
        // 是否有必要
        // private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        public UserCreateSendWelcomeEventHandler(
            ISettingProvider settingProvider,
            INotificationStore notificationStore,
            INotificationDispatcher notificationDispatcher,
            IStringLocalizer<MessageServiceResource> stringLocalizer
            //ICurrentPrincipalAccessor currentPrincipalAccessor
            )
        {
            _settingProvider = settingProvider;
            _stringLocalizer = stringLocalizer;
            _notificationStore = notificationStore;
            _notificationDispatcher = notificationDispatcher;

            //_currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            // 获取默认语言
            var userDefaultCultureName = await _settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            if (!userDefaultCultureName.IsNullOrWhiteSpace())
            {
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(userDefaultCultureName);
                // CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(userDefaultCultureName);
            }
            // 订阅用户欢迎消息
            await _notificationStore.InsertUserSubscriptionAsync(eventData.Entity.TenantId,
                eventData.Entity.Id, UserNotificationNames.WelcomeToApplication);

            var userWelcomeNotifiction = new NotificationInfo
            {
                CreationTime = DateTime.Now,
                Name = UserNotificationNames.WelcomeToApplication,
                NotificationSeverity = NotificationSeverity.Info,
                NotificationType = NotificationType.System,
                TenantId = eventData.Entity.TenantId
            };
            userWelcomeNotifiction.Data.Properties["message"] = L("WelcomeToApplicationFormUser", eventData.Entity.UserName);

            await _notificationDispatcher.DispatcheAsync(userWelcomeNotifiction);
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

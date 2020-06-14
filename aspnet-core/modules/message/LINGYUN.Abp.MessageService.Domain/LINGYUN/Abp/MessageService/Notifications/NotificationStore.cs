using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.MessageService.Utils;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationStore : DomainService, INotificationStore
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private IJsonSerializer _jsonSerializer;
        protected IJsonSerializer JsonSerializer => LazyGetRequiredService(ref _jsonSerializer);

        private ISnowflakeIdGenerator _snowflakeIdGenerator;
        protected ISnowflakeIdGenerator SnowflakeIdGenerator => LazyGetRequiredService(ref _snowflakeIdGenerator);

        private INotificationRepository _notificationRepository;
        protected INotificationRepository NotificationRepository => LazyGetRequiredService(ref _notificationRepository);

        private IUserNotificationRepository _userNotificationRepository;
        protected IUserNotificationRepository UserNotificationRepository => LazyGetRequiredService(ref _userNotificationRepository);

        private IUserSubscribeRepository _userSubscribeRepository;
        protected IUserSubscribeRepository UserSubscribeRepository => LazyGetRequiredService(ref _userSubscribeRepository);

        public NotificationStore(
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task ChangeUserNotificationReadStateAsync(Guid? tenantId, Guid userId, long notificationId, NotificationReadState readState)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(tenantId))
            {
                await UserNotificationRepository.ChangeUserNotificationReadStateAsync(userId, notificationId, readState);

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task DeleteNotificationAsync(NotificationInfo notification)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(notification.TenantId))
            {
                var notify = await NotificationRepository.GetByIdAsync(notification.GetId());
                await NotificationRepository.DeleteAsync(notify.Id);

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task DeleteUserNotificationAsync(Guid? tenantId, Guid userId, long notificationId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(tenantId))
            {
                var notify = await UserNotificationRepository.GetByIdAsync(userId, notificationId);
                await UserNotificationRepository.DeleteAsync(notify.Id);

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task DeleteUserSubscriptionAsync(Guid? tenantId, Guid userId, string notificationName)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(tenantId))
            {
                var userSubscribe = await UserSubscribeRepository.GetUserSubscribeAsync(notificationName, userId);
                await UserSubscribeRepository.DeleteAsync(userSubscribe.Id);

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<NotificationInfo> GetNotificationOrNullAsync(Guid? tenantId, long notificationId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var notification = await NotificationRepository.GetByIdAsync(notificationId);

                return _objectMapper.Map<Notification, NotificationInfo>(notification);
            }
        }

        public async Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(Guid? tenantId, string notificationName)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userSubscriptions = await UserSubscribeRepository.GetSubscribesAsync(notificationName);

                return _objectMapper.Map<List<UserSubscribe>, List<NotificationSubscriptionInfo>>(userSubscriptions);
            }
        }

        public async Task<List<NotificationInfo>> GetUserNotificationsAsync(Guid? tenantId, Guid userId, NotificationReadState readState = NotificationReadState.UnRead, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var notifications = await UserNotificationRepository.GetNotificationsAsync(userId, readState, maxResultCount);

                return _objectMapper.Map<List<Notification>, List<NotificationInfo>>(notifications);
            }
        }

        public async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, Guid userId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userSubscriptionNames = await UserSubscribeRepository.GetUserSubscribesAsync(userId);

                var userSubscriptions = new List<NotificationSubscriptionInfo>();

                userSubscriptionNames.ForEach(name => userSubscriptions.Add(
                    new NotificationSubscriptionInfo
                    {
                        UserId = userId,
                        TenantId = tenantId,
                        NotificationName = name
                    }));

                return userSubscriptions;
            }
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, string userName)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userSubscriptions = await UserSubscribeRepository.GetUserSubscribesByNameAsync(userName);

                var userSubscriptionInfos = new List<NotificationSubscriptionInfo>();

                userSubscriptions.ForEach(us => userSubscriptionInfos.Add(
                    new NotificationSubscriptionInfo
                    {
                        UserId = us.UserId,
                        TenantId = us.TenantId,
                        NotificationName = us.NotificationName
                    }));

                return userSubscriptionInfos;
            }
        }

        [UnitOfWork]
        public async Task InsertNotificationAsync(NotificationInfo notification)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(notification.TenantId))
            {
                // var notifyId = notification.GetId();
                var notifyId = SnowflakeIdGenerator.Create();
                // 保存主键，防止前端js long类型溢出
                // notification.Data["id"] = notifyId.ToString();

                var notify = new Notification(notifyId, notification.Name,
                    notification.Data.GetType().AssemblyQualifiedName,
                    JsonSerializer.Serialize(notification.Data), notification.NotificationSeverity)
                {
                    CreationTime = Clock.Now,
                    Type = notification.NotificationType,
                    ExpirationTime = Clock.Now.AddDays(60)
                };
                notify.SetTenantId(notification.TenantId);

                await NotificationRepository.InsertAsync(notify);

                notification.Id = notify.NotificationId.ToString();

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task InsertUserNotificationAsync(NotificationInfo notification, Guid userId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(notification.TenantId))
            {
                var userNotification = new UserNotification(notification.GetId(), userId, notification.TenantId);
                await UserNotificationRepository.InsertAsync(userNotification);

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task InsertUserSubscriptionAsync(Guid? tenantId, UserIdentifier identifier, string notificationName)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(tenantId))
            {

                var userSubscription = new UserSubscribe(notificationName, identifier.UserId, identifier.UserName, tenantId)
                {
                    CreationTime = Clock.Now
                };

                await UserSubscribeRepository.InsertAsync(userSubscription);

                await unitOfWork.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public async Task InsertUserSubscriptionAsync(Guid? tenantId, IEnumerable<UserIdentifier> identifiers, string notificationName)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(tenantId))
            {
                var userSubscribes = new List<UserSubscribe>();

                foreach (var identifier in identifiers)
                {
                    userSubscribes.Add(new UserSubscribe(notificationName, identifier.UserId, identifier.UserName, tenantId));
                }

                await UserSubscribeRepository.InsertUserSubscriptionAsync(userSubscribes);

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> IsSubscribedAsync(Guid? tenantId, Guid userId, string notificationName)
        {
            using (CurrentTenant.Change(tenantId))
            return await UserSubscribeRepository.UserSubscribeExistsAysnc(notificationName, userId);
        }

        [UnitOfWork]
        public async Task InsertUserNotificationsAsync(NotificationInfo notification, IEnumerable<Guid> userIds)
        {
            // 添加工作单元
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (CurrentTenant.Change(notification.TenantId))
            {
                var userNofitications = new List<UserNotification>();
                foreach (var userId in userIds)
                {
                    var userNofitication = new UserNotification(notification.GetId(), userId, notification.TenantId);
                    userNofitications.Add(userNofitication);
                }
                await UserNotificationRepository.InsertUserNotificationsAsync(userNofitications);

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}

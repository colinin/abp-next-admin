using LINGYUN.Abp.MessageService.Subscriptions;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(INotificationStore))]
    public class NotificationStore : INotificationStore
    {
        private readonly IClock _clock;

        private readonly IObjectMapper _objectMapper;

        private readonly ICurrentTenant _currentTenant;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly INotificationRepository _notificationRepository;

        private readonly IUserNotificationRepository _userNotificationRepository;

        private readonly IUserSubscribeRepository _userSubscribeRepository;

        public NotificationStore(
            IClock clock,
            IObjectMapper objectMapper,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            INotificationRepository notificationRepository,
            IUserSubscribeRepository userSubscribeRepository,
            IUserNotificationRepository userNotificationRepository
            )
        {
            _clock = clock;
            _objectMapper = objectMapper;
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _notificationRepository = notificationRepository;
            _userSubscribeRepository = userSubscribeRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public virtual async Task ChangeUserNotificationReadStateAsync(
            Guid? tenantId,
            Guid userId,
            long notificationId,
            NotificationReadState readState,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                var notification = await _userNotificationRepository.GetByIdAsync(userId, notificationId);
                if (notification != null)
                {
                    notification.ChangeReadState(readState);
                    await _userNotificationRepository.UpdateAsync(notification);

                    await unitOfWork.CompleteAsync();
                }
            }
        }

        public virtual async Task DeleteNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(notification.TenantId))
            {
                var notify = await _notificationRepository.GetByIdAsync(notification.GetId(), cancellationToken);
                await _notificationRepository.DeleteAsync(notify.Id, cancellationToken: cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteNotificationAsync(
            int batchCount,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _notificationRepository.DeleteExpritionAsync(batchCount, cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteUserNotificationAsync(
            Guid? tenantId,
            Guid userId,
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                var notify = await _userNotificationRepository
                    .GetByIdAsync(userId, notificationId, cancellationToken);
                await _userNotificationRepository
                    .DeleteAsync(notify.Id, cancellationToken: cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAllUserSubscriptionAsync(
            Guid? tenantId,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                await _userSubscribeRepository
                    .DeleteUserSubscriptionAsync(notificationName, cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteUserSubscriptionAsync(
            Guid? tenantId,
            Guid userId,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                var userSubscribe = await _userSubscribeRepository
                    .GetUserSubscribeAsync(notificationName, userId, cancellationToken);
                await _userSubscribeRepository
                    .DeleteAsync(userSubscribe.Id, cancellationToken: cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteUserSubscriptionAsync(
            Guid? tenantId,
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                await _userSubscribeRepository
                    .DeleteUserSubscriptionAsync(notificationName, identifiers.Select(ids => ids.UserId), cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task<NotificationInfo> GetNotificationOrNullAsync(
            Guid? tenantId,
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var notification = await _notificationRepository
                    .GetByIdAsync(notificationId, cancellationToken);

                return _objectMapper.Map<Notification, NotificationInfo>(notification);
            }
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId,
            string notificationName,
            IEnumerable<UserIdentifier> identifiers = null,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userSubscriptions = new List<UserSubscribe>();

                if (identifiers == null)
                {
                    userSubscriptions = await _userSubscribeRepository
                        .GetUserSubscribesAsync(notificationName, null, cancellationToken);
                }
                else
                {
                    userSubscriptions = await _userSubscribeRepository
                        .GetUserSubscribesAsync(
                            notificationName,
                            identifiers.Select(ids => ids.UserId),
                            cancellationToken);
                }

                return _objectMapper.Map<List<UserSubscribe>, List<NotificationSubscriptionInfo>>(userSubscriptions);
            }
        }

        public virtual async Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId,
            Guid userId,
            NotificationReadState? readState = null,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var notifications = await _userNotificationRepository
                    .GetNotificationsAsync(userId, readState, maxResultCount, cancellationToken);

                return _objectMapper.Map<List<Notification>, List<NotificationInfo>>(notifications);
            }
        }

        public virtual async Task<int> GetUserNotificationsCountAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            NotificationReadState? readState = null,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userNotificationRepository
                    .GetCountAsync(userId, filter, readState, cancellationToken);
            }
        }

        public virtual async Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(NotificationInfo.CreationTime),
            NotificationReadState? readState = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var notifications = await _userNotificationRepository
                    .GetListAsync(userId, filter, sorting, readState, skipCount, maxResultCount, cancellationToken);

                return _objectMapper.Map<List<Notification>, List<NotificationInfo>>(notifications);
            }
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userSubscriptionNames = await _userSubscribeRepository
                    .GetUserSubscribesAsync(userId, cancellationToken);

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

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId,
            string userName,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userSubscriptions = await _userSubscribeRepository
                    .GetUserSubscribesByNameAsync(userName, cancellationToken);

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

        public virtual async Task InsertNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(notification.TenantId))
            {
                var notify = new Notification(
                    notification.GetId(),
                    notification.Name,
                    notification.Data.GetType().AssemblyQualifiedName,
                    notification.Data,
                    notification.Severity,
                    notification.TenantId)
                {
                    CreationTime = _clock.Now,
                    Type = notification.Type,
                    // TODO: 通知过期时间应该可以配置
                    ExpirationTime = _clock.Now.AddDays(60)
                };

                await _notificationRepository.InsertAsync(notify, cancellationToken: cancellationToken);

                notification.SetId(notify.NotificationId);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task InsertUserNotificationAsync(
            NotificationInfo notification,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(notification.TenantId))
            {
                var userNotification = new UserNotification(notification.GetId(), userId, notification.TenantId);
                await _userNotificationRepository
                    .InsertAsync(userNotification, cancellationToken: cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task InsertUserSubscriptionAsync(
            Guid? tenantId,
            UserIdentifier identifier,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {

                var userSubscription = new UserSubscribe(notificationName, identifier.UserId, identifier.UserName, tenantId)
                {
                    CreationTime = _clock.Now
                };

                await _userSubscribeRepository
                    .InsertAsync(userSubscription, cancellationToken: cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task InsertUserSubscriptionAsync(
            Guid? tenantId,
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(tenantId))
            {
                var userSubscribes = new List<UserSubscribe>();

                foreach (var identifier in identifiers)
                {
                    userSubscribes.Add(new UserSubscribe(notificationName, identifier.UserId, identifier.UserName, tenantId));
                }

                await _userSubscribeRepository
                    .InsertUserSubscriptionAsync(userSubscribes, cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        public virtual async Task<bool> IsSubscribedAsync(
            Guid? tenantId,
            Guid userId,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
                return await _userSubscribeRepository
                        .UserSubscribeExistsAysnc(notificationName, userId, cancellationToken);
        }

        public virtual async Task InsertUserNotificationsAsync(
            NotificationInfo notification,
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            using (_currentTenant.Change(notification.TenantId))
            {
                var userNofitications = new List<UserNotification>();
                foreach (var userId in userIds)
                {
                    // 重复检查
                    // TODO:如果存在很多个订阅用户,这是个隐患
                    if (!await _userNotificationRepository.AnyAsync(userId, notification.GetId(), cancellationToken))
                    {
                        var userNofitication = new UserNotification(notification.GetId(), userId, notification.TenantId);
                        userNofitications.Add(userNofitication);
                    }
                }
                await _userNotificationRepository
                    .InsertUserNotificationsAsync(userNofitications, cancellationToken);

                await unitOfWork.CompleteAsync(cancellationToken);
            }
        }
    }
}

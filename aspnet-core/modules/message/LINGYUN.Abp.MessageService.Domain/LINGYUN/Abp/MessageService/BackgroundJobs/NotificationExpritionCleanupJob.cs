using LINGYUN.Abp.MessageService.Notifications;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.BackgroundJobs
{
    public class NotificationExpritionCleanupJob : AsyncBackgroundJob<CleanupNotificationJobArgs>
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly INotificationRepository _notificationRepository;
        public NotificationExpritionCleanupJob(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            INotificationRepository notificationRepository)
        {
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _notificationRepository = notificationRepository;
        }

        public override async Task ExecuteAsync(CleanupNotificationJobArgs args)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                using (_currentTenant.Change(args.TenantId))
                {
                    await _notificationRepository.DeleteExpritionAsync(args.Count);

                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}

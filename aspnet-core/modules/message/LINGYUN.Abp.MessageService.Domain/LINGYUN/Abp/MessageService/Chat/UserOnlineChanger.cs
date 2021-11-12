using LINGYUN.Abp.IM;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserOnlineChanger : IUserOnlineChanger, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserChatCardRepository _userChatCardRepository;

        public UserOnlineChanger(
            IClock clock,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IUserChatCardRepository userChatCardRepository)
        {
            _clock = clock;
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _userChatCardRepository = userChatCardRepository;
        }

        public virtual async Task ChangeAsync(
            Guid? tenantId,
            Guid userId,
            UserOnlineState state,
            CancellationToken cancellationToken = default)
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            using (_currentTenant.Change(tenantId))
            {
                var userChatCard = await _userChatCardRepository.FindByUserIdAsync(userId);
                userChatCard?.ChangeState(_clock, state);

                await unitOfWork.CompleteAsync();
            }
        }
    }
}

using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreUserChatSettingRepository : EfCoreRepository<IMessageServiceDbContext, UserChatSetting, long>,
        IUserChatSettingRepository, ITransientDependency
    {
        public EfCoreUserChatSettingRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserChatSetting> GetByUserIdAsync(Guid userId)
        {
            return await DbSet.Where(x => x.UserId.Equals(userId))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserHasBlackedAsync(Guid formUserId, Guid toUserId)
        {
            return await DbContext.Set<UserChatBlack>()
                .AnyAsync(x => x.UserId.Equals(toUserId) && x.ShieldUserId.Equals(formUserId));
        }

        public async Task<bool> UserHasOpendImAsync(Guid userId)
        {
            return await DbSet.AnyAsync(x => x.UserId.Equals(userId));
        }
    }
}

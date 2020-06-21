using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Messages
{
    public class EfCoreGroupRepository : EfCoreRepository<MessageServiceDbContext, ChatGroup, long>,
        IGroupRepository, ITransientDependency
    {
        public EfCoreGroupRepository(
            IDbContextProvider<MessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<ChatGroup> GetByIdAsync(long id)
        {
            return await DbSet.Where(x => x.GroupId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<ChatGroupAdmin>> GetGroupAdminAsync(long id)
        {
            var groupAdmins = await (from gp in DbContext.Set<ChatGroup>()
                                     join gpa in DbContext.Set<ChatGroupAdmin>()
                                       on gp.GroupId equals gpa.GroupId
                                     select gpa)
                                     .Distinct()
                                     .ToListAsync();
            return groupAdmins;
        }

        public async Task<bool> UserHasBlackedAsync(long id, Guid formUserId)
        {
            var userHasBlack = await DbContext.Set<GroupChatBlack>()
                .AnyAsync(x => x.GroupId.Equals(id) && x.ShieldUserId.Equals(formUserId));
            return userHasBlack;
        }
    }
}

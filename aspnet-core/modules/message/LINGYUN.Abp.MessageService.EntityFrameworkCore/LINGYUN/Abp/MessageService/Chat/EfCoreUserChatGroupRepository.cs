using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreUserChatGroupRepository : EfCoreRepository<MessageServiceDbContext, UserChatGroup, long>,
        IUserChatGroupRepository, ITransientDependency
    {
        public EfCoreUserChatGroupRepository(
            IDbContextProvider<MessageServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<UserGroup>> GetGroupUsersAsync(long groupId)
        {
            // TODO: 急需单元测试,对这段代码不是太自信...
            var groupUsers = await (from ug in DbSet
                                    join x in (
                                       from gaj in DbContext.Set<ChatGroupAdmin>()
                                       where gaj.GroupId.Equals(groupId)
                                       select gaj
                                    ) on ug.UserId equals x.UserId
                                    into y from ga in y.DefaultIfEmpty()
                                    where ug.GroupId.Equals(groupId)
                                    select new UserGroup
                                    {
                                        GroupId = ug.GroupId,
                                        IsSuperAdmin = ga != null && ga.IsSuperAdmin,
                                        IsAdmin = ga != null,
                                        TenantId = ug.TenantId,
                                        UserId = ug.UserId
                                    })
                                    .Distinct()
                                    .AsNoTracking()
                                    .ToListAsync();
            return groupUsers;
        }

        public async Task<GroupUserCard> GetGroupUserCardAsync(long groupId, Guid userId)
        {
            var groupUserCard = await (from cg in DbContext.Set<ChatGroup>()
                                       join ucg in DbContext.Set<UserChatGroup>().DefaultIfEmpty()
                                          on cg.GroupId equals ucg.GroupId
                                       join cga in DbContext.Set<ChatGroupAdmin>().DefaultIfEmpty()
                                          on cg.GroupId equals cga.GroupId
                                       where ucg.GroupId.Equals(groupId) && cga.UserId.Equals(userId)
                                       select new GroupUserCard
                                       {
                                           IsSuperAdmin = cga != null && cga.IsSuperAdmin,
                                           IsAdmin = cga != null,//能查到数据就是管理员
                                           GroupId = ucg.GroupId,
                                           UserId = ucg.UserId,
                                           TenantId = ucg.TenantId,
                                           Permissions = new Dictionary<string, bool>
                                           {
                                               { nameof(ChatGroupAdmin.AllowAddPeople), cga != null &&cga.AllowAddPeople },
                                               { nameof(ChatGroupAdmin.AllowDissolveGroup), cga != null &&cga.AllowDissolveGroup },
                                               { nameof(ChatGroupAdmin.AllowKickPeople), cga != null &&cga.AllowKickPeople },
                                               { nameof(ChatGroupAdmin.AllowSendNotice), cga != null &&cga.AllowSendNotice },
                                               { nameof(ChatGroupAdmin.AllowSilence), cga != null &&cga.AllowSilence },
                                               { nameof(ChatGroupAdmin.IsSuperAdmin), cga != null &&cga.IsSuperAdmin }
                                           }
                                       })
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync();

            return groupUserCard;
        }

        public Task<List<UserGroup>> GetGroupUsersAsync(long groupId, string filter = "", string sorting = "UserId", int skipCount = 1, int maxResultCount = 10)
        {
            // TODO: 复杂的实现，暂时无关紧要，后期再说 :)
            throw new NotImplementedException();
        }

        public Task<int> GetGroupUsersCountAsync(long groupId, string filter = "")
        {
            // TODO: 复杂的实现，暂时无关紧要，后期再说 :)
            throw new NotImplementedException();
        }

        public async Task<UserChatGroup> GetUserGroupAsync(long groupId, Guid userId)
        {
            return await DbSet.Where(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<Group>> GetUserGroupsAsync(Guid userId)
        {
            // TODO: 急需单元测试,对这段代码不是太自信...
            var userGroups = await (from ucg in DbSet
                                    join cg in DbContext.Set<ChatGroup>()
                                       on ucg.GroupId equals cg.GroupId
                                    group cg by new
                                    {
                                        cg.GroupId,
                                        cg.Name,
                                        cg.AllowAnonymous,
                                        cg.AllowSendMessage,
                                        cg.MaxUserCount
                                    }
                             into ug
                                    orderby ug.Key.GroupId descending
                                    select new Group
                                    {
                                        AllowAnonymous = ug.Key.AllowAnonymous,
                                        AllowSendMessage = ug.Key.AllowSendMessage,
                                        GroupUserCount = ug.Count(),
                                        MaxUserLength = ug.Key.MaxUserCount,
                                        Name = ug.Key.Name
                                    })
                                    .Distinct()
                                    .ToListAsync();

            return userGroups;
        }

        public async Task<bool> UserHasInGroupAsync(long groupId, Guid userId)
        {
            return await DbSet.AnyAsync(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId));
        }
    }
}

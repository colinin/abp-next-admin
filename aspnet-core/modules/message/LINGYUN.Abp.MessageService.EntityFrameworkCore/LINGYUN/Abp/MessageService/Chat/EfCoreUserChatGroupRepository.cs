using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreUserChatGroupRepository : EfCoreRepository<IMessageServiceDbContext, UserChatGroup, long>,
        IUserChatGroupRepository, ITransientDependency
    {
        public EfCoreUserChatGroupRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<GroupUserCard> GetMemberAsync(
            long groupId, 
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var cardQuery = from gp in DbContext.Set<ChatGroup>()
                       join ucg in DbContext.Set<UserChatGroup>()
                            on gp.GroupId equals ucg.GroupId
                       join ugc in DbContext.Set<UserGroupCard>()
                            on ucg.UserId equals ugc.UserId
                       join uc in DbContext.Set<UserChatCard>()
                            on ugc.UserId equals uc.UserId
                       where gp.GroupId == groupId && ugc.UserId == userId
                       select new GroupUserCard
                       {
                           TenantId = uc.TenantId,
                           UserId = uc.UserId,
                           UserName = uc.UserName,
                           Age = uc.Age,
                           AvatarUrl = uc.AvatarUrl,
                           IsAdmin = ugc.IsAdmin,
                           IsSuperAdmin = gp.AdminUserId == uc.UserId,
                           GroupId = gp.GroupId,
                           Birthday = uc.Birthday,
                           Description = uc.Description,
                           NickName = ugc.NickName ?? uc.NickName,
                           Sex = uc.Sex,
                           Sign = uc.Sign
                       };

            return await cardQuery
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<GroupUserCard>> GetMembersAsync(
            long groupId, 
            string sorting = nameof(UserChatCard.UserId), 
            bool reverse = false, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default)
        {
            sorting ??= nameof(UserChatCard.UserId);
            sorting = reverse ? sorting + " desc" : sorting;
            var cardQuery = from gp in DbContext.Set<ChatGroup>()
                             join ucg in DbContext.Set<UserChatGroup>()
                                  on gp.GroupId equals ucg.GroupId
                             join ugc in DbContext.Set<UserGroupCard>()
                                  on ucg.UserId equals ugc.UserId
                             join uc in DbContext.Set<UserChatCard>()
                                  on ugc.UserId equals uc.UserId
                             where gp.GroupId == groupId
                             select new GroupUserCard
                             {
                                 TenantId = uc.TenantId,
                                 UserId = uc.UserId,
                                 UserName = uc.UserName,
                                 Age = uc.Age,
                                 AvatarUrl = uc.AvatarUrl,
                                 IsAdmin = ugc.IsAdmin,
                                 IsSuperAdmin = gp.AdminUserId == uc.UserId,
                                 GroupId = gp.GroupId,
                                 Birthday = uc.Birthday,
                                 Description = uc.Description,
                                 NickName = ugc.NickName ?? uc.NickName,
                                 Sex = uc.Sex,
                                 Sign = uc.Sign
                             };

            return await cardQuery
                .OrderBy(sorting ?? nameof(UserChatCard.UserId))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetMembersCountAsync(
            long groupId, 
            CancellationToken cancellationToken = default)
        {
            var cardQuery = from gp in DbContext.Set<ChatGroup>()
                            join ucg in DbContext.Set<UserChatGroup>()
                                 on gp.GroupId equals ucg.GroupId
                            join ugc in DbContext.Set<UserGroupCard>()
                                 on ucg.UserId equals ugc.UserId
                            join uc in DbContext.Set<UserChatCard>()
                                 on ugc.UserId equals uc.UserId
                            where gp.GroupId == groupId
                            select ucg;

            return await cardQuery
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> MemberHasInGroupAsync(
            long groupId, 
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<UserChatGroup>()
                .AnyAsync(ucg => ucg.GroupId == groupId && ucg.UserId == userId,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Group>> GetMemberGroupsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var groupQuery = from gp in DbContext.Set<ChatGroup>()
                             join ucg in DbContext.Set<UserChatGroup>()
                                  on gp.GroupId equals ucg.GroupId
                             where ucg.UserId.Equals(userId)
                             group ucg by new
                             {
                                 gp.AllowAnonymous,
                                 gp.AllowSendMessage,
                                 gp.MaxUserCount,
                                 gp.Name
                             }
                             into cg
                             select new Group
                             {
                                 AllowAnonymous = cg.Key.AllowAnonymous,
                                 AllowSendMessage = cg.Key.AllowSendMessage,
                                 MaxUserLength = cg.Key.MaxUserCount,
                                 Name = cg.Key.Name,
                                 GroupUserCount = cg.Count()
                             };

            return await groupQuery
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task RemoveMemberFormGroupAsync(
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            await DeleteAsync(ucg => ucg.GroupId == groupId && ucg.UserId == userId);
        }

        //public virtual async Task<List<GroupUserCard>> GetGroupUsersAsync(
        //    long groupId,
        //    CancellationToken cancellationToken = default)
        //{
        //    // TODO: 急需单元测试,对这段代码不是太自信...
        //    var groupUsers = await (from cg in DbContext.Set<ChatGroup>()
        //                            join ucg in DbContext.Set<UserChatGroup>()
        //                                on cg.GroupId equals ucg.GroupId
        //                            join ugc in DbContext.Set<UserGroupCard>()
        //                                on ucg.UserId equals ugc.UserId
        //                            where cg.GroupId.Equals(groupId)
        //                            select new GroupUserCard
        //                            {
        //                                GroupId = ucg.GroupId,
        //                                IsSuperAdmin = ugc.UserId == cg.AdminUserId,
        //                                IsAdmin = ugc.IsAdmin,
        //                                TenantId = ucg.TenantId,
        //                                UserId = ucg.UserId
        //                            })
        //                            .Distinct()
        //                            .AsNoTracking()
        //                            .ToListAsync(GetCancellationToken(cancellationToken));
        //    return groupUsers;
        //}

        //public virtual async Task<GroupUserCard> GetMemberAsync(
        //    long groupId, 
        //    Guid userId,
        //    CancellationToken cancellationToken = default)
        //{
        //    var groupUserCard = await (from cg in DbContext.Set<ChatGroup>()
        //                               join ucg in DbContext.Set<UserChatGroup>().DefaultIfEmpty()
        //                                  on cg.GroupId equals ucg.GroupId
        //                               join ucc in DbContext.Set<UserChatCard>().DefaultIfEmpty()
        //                                  on ucg.UserId equals ucc.UserId
        //                               join cga in DbContext.Set<ChatGroupAdmin>().DefaultIfEmpty()
        //                                  on cg.GroupId equals cga.GroupId
        //                               where ucg.GroupId.Equals(groupId) && cga.UserId.Equals(userId)
        //                               select new GroupUserCard
        //                               {
        //                                   IsSuperAdmin = cga != null && cga.IsSuperAdmin,
        //                                   IsAdmin = cga != null,//能查到数据就是管理员
        //                                   GroupId = ucg.GroupId,
        //                                   UserId = ucg.UserId,
        //                                   TenantId = ucg.TenantId,
        //                                   Permissions = new Dictionary<string, bool>
        //                                   {
        //                                       { nameof(ChatGroupAdmin.AllowAddPeople), cga != null && cga.AllowAddPeople },
        //                                       { nameof(ChatGroupAdmin.AllowDissolveGroup), cga != null && cga.AllowDissolveGroup },
        //                                       { nameof(ChatGroupAdmin.AllowKickPeople), cga != null && cga.AllowKickPeople },
        //                                       { nameof(ChatGroupAdmin.AllowSendNotice), cga != null && cga.AllowSendNotice },
        //                                       { nameof(ChatGroupAdmin.AllowSilence), cga != null && cga.AllowSilence },
        //                                       { nameof(ChatGroupAdmin.IsSuperAdmin), cga != null && cga.IsSuperAdmin }
        //                                   }
        //                               })
        //                         .AsNoTracking()
        //                         .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        //    return groupUserCard;
        //}

        //public virtual Task<List<UserGroup>> GetUsersAsync(
        //    long groupId, 
        //    string filter = "", 
        //    string sorting = nameof(UserGroup.UserId),
        //    bool reverse = false,
        //    int skipCount = 0, 
        //    int maxResultCount = 10,
        //    CancellationToken cancellationToken = default)
        //{
        //    sorting = reverse ? sorting + " desc" : sorting;
        //    // TODO: 复杂的实现，暂时无关紧要，后期再说 :)
        //    throw new NotImplementedException();
        //}

        //public virtual Task<int> GetMembersCountAsync(
        //    long groupId,
        //    string filter = "",
        //    CancellationToken cancellationToken = default)
        //{
        //    var ss = (from ucg in DbContext.Set<UserChatGroup>()
        //             join cg in DbContext.Set<ChatGroup>() on ucg.GroupId equals cg.GroupId
        //             select cg)
        //             .WhereIf(!filter.IsNullOrWhiteSpace(),)

        //    // TODO: 复杂的实现，暂时无关紧要，后期再说 :)
        //    //throw new NotImplementedException();
        //}

        //public virtual async Task<UserChatGroup> GetUserGroupAsync(
        //    long groupId, 
        //    Guid userId,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await DbSet.Where(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId))
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        //}

        //public virtual async Task<List<Group>> GetUserGroupsAsync(
        //    Guid userId,
        //    CancellationToken cancellationToken = default)
        //{
        //    // TODO: 急需单元测试,对这段代码不是太自信...
        //    var userGroups = await (from ucg in DbSet
        //                            join cg in DbContext.Set<ChatGroup>()
        //                               on ucg.GroupId equals cg.GroupId
        //                            group cg by new
        //                            {
        //                                cg.GroupId,
        //                                cg.Name,
        //                                cg.AllowAnonymous,
        //                                cg.AllowSendMessage,
        //                                cg.MaxUserCount
        //                            }
        //                     into ug
        //                            orderby ug.Key.GroupId descending
        //                            select new Group
        //                            {
        //                                AllowAnonymous = ug.Key.AllowAnonymous,
        //                                AllowSendMessage = ug.Key.AllowSendMessage,
        //                                GroupUserCount = ug.Count(),
        //                                MaxUserLength = ug.Key.MaxUserCount,
        //                                Name = ug.Key.Name
        //                            })
        //                            .Distinct()
        //                            .ToListAsync(GetCancellationToken(cancellationToken));

        //    return userGroups;
        //}

        //public virtual async Task<bool> MemberHasInGroupAsync(
        //    long groupId,
        //    Guid userId,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await DbSet
        //        .AnyAsync(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId), GetCancellationToken(cancellationToken));
        //}
    }
}

using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.MessageService.Chat;
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

namespace LINGYUN.Abp.MessageService.Group
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

        public virtual async Task<List<IM.Group.Group>> GetMemberGroupsAsync(
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
                             select new IM.Group.Group
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
            await DeleteAsync(ucg => ucg.GroupId == groupId && ucg.UserId == userId, cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}

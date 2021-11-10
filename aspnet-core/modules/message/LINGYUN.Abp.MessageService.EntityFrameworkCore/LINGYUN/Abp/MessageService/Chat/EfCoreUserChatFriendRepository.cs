using LINGYUN.Abp.IM;
using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreUserChatFriendRepository : EfCoreRepository<IMessageServiceDbContext, UserChatFriend, long>, IUserChatFriendRepository
    {
        public EfCoreUserChatFriendRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<UserChatFriend> FindByUserFriendIdAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(ucf => ucf.UserId == userId && ucf.FrientId == friendId)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserFriend>> GetAllMembersAsync(
            Guid userId,
            string sorting = nameof(UserChatFriend.RemarkName),
             CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userFriendQuery = from ucf in dbContext.Set<UserChatFriend>()
                                  join ucc in dbContext.Set<UserChatCard>()
                                  //      on ucf.FrientId equals ucc.UserId // 查询双向好友的
                                        on ucf.UserId equals ucc.UserId
                                  where ucf.UserId == userId && ucf.Status == UserFriendStatus.Added
                                  select new UserFriend
                                  {
                                      Age = ucc.Age,
                                      AvatarUrl = ucc.AvatarUrl,
                                      Birthday = ucc.Birthday,
                                      Black = ucf.Black,
                                      Description = ucc.Description,
                                      DontDisturb = ucf.DontDisturb,
                                      FriendId = ucf.FrientId,
                                      NickName = ucc.NickName,
                                      RemarkName = ucf.RemarkName ?? ucc.NickName,
                                      Sex = ucc.Sex,
                                      Sign = ucc.Sign,
                                      SpecialFocus = ucf.SpecialFocus,
                                      TenantId = ucf.TenantId,
                                      UserId = ucf.UserId,
                                      UserName = ucc.UserName,
                                      Online = ucc.State == UserOnlineState.Online,
                                  };

            return await userFriendQuery
                .OrderBy(sorting ?? nameof(UserChatFriend.RemarkName))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<UserFriend> GetMemberAsync(Guid userId, Guid friendId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userFriendQuery = from ucf in dbContext.Set<UserChatFriend>()
                                  join ucc in dbContext.Set<UserChatCard>()
                                        on ucf.FrientId equals ucc.UserId
                                  where ucf.UserId == userId && ucf.FrientId == friendId && ucf.Status == UserFriendStatus.Added
                                  select new UserFriend
                                  {
                                      Age = ucc.Age,
                                      AvatarUrl = ucc.AvatarUrl,
                                      Birthday = ucc.Birthday,
                                      Black = ucf.Black,
                                      Description = ucc.Description,
                                      DontDisturb = ucf.DontDisturb,
                                      FriendId = ucf.FrientId,
                                      NickName = ucc.NickName,
                                      RemarkName = ucf.RemarkName,
                                      Sex = ucc.Sex,
                                      Sign = ucc.Sign,
                                      SpecialFocus = ucf.SpecialFocus,
                                      TenantId = ucf.TenantId,
                                      UserId = ucf.UserId,
                                      UserName = ucc.UserName,
                                      Online = ucc.State == UserOnlineState.Online,
                                  };

            return await userFriendQuery
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserFriend>> GetMembersAsync(
            Guid userId,
            string filter = "",
            string sorting = nameof(UserChatFriend.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            // 过滤用户资料
            var userChatCardQuery = dbContext.Set<UserChatCard>()
                .WhereIf(!filter.IsNullOrWhiteSpace(), ucc => ucc.UserName.Contains(filter) || ucc.NickName.Contains(filter));

            // 过滤好友资料
            var userChatFriendQuery = dbContext.Set<UserChatFriend>()
                .Where(ucf => ucf.Status == UserFriendStatus.Added)
                .WhereIf(!filter.IsNullOrWhiteSpace(), ucf => ucf.RemarkName.Contains(filter));

            // 组合查询
            var userFriendQuery = from ucf in userChatFriendQuery
                                  join ucc in userChatCardQuery // TODO: Need LEFT JOIN?
                                        on ucf.FrientId equals ucc.UserId
                                  where ucf.UserId == userId
                                  select new UserFriend
                                  {
                                      Age = ucc.Age,
                                      AvatarUrl = ucc.AvatarUrl,
                                      Birthday = ucc.Birthday,
                                      Black = ucf.Black,
                                      Description = ucc.Description,
                                      DontDisturb = ucf.DontDisturb,
                                      FriendId = ucf.FrientId,
                                      NickName = ucc.NickName,
                                      RemarkName = ucf.RemarkName,
                                      Sex = ucc.Sex,
                                      Sign = ucc.Sign,
                                      SpecialFocus = ucf.SpecialFocus,
                                      TenantId = ucf.TenantId,
                                      UserId = ucf.UserId,
                                      UserName = ucc.UserName,
                                      Online = ucc.State == UserOnlineState.Online,
                                  };

            return await userFriendQuery
                .OrderBy(sorting ?? nameof(UserChatFriend.UserId))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserFriend>> GetLastContactMembersAsync(
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10,
             CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userReceiveMsgQuery = dbContext.Set<UserMessage>()
                .Where(um => um.ReceiveUserId == userId);

            var userFriendQuery = from ucf in dbContext.Set<UserChatFriend>()
                                  join ucc in dbContext.Set<UserChatCard>()
                                        on ucf.FrientId equals ucc.UserId
                                  join um in userReceiveMsgQuery
                                        on ucc.UserId equals um.CreatorId
                                  where ucf.UserId == userId && ucf.Status == UserFriendStatus.Added
                                  orderby um.CreationTime descending // 消息创建时间倒序
                                  select new UserFriend
                                  {
                                      Age = ucc.Age,
                                      AvatarUrl = ucc.AvatarUrl,
                                      Birthday = ucc.Birthday,
                                      Black = ucf.Black,
                                      Description = ucc.Description,
                                      DontDisturb = ucf.DontDisturb,
                                      FriendId = ucf.FrientId,
                                      NickName = ucc.NickName,
                                      RemarkName = ucf.RemarkName,
                                      Sex = ucc.Sex,
                                      Sign = ucc.Sign,
                                      SpecialFocus = ucf.SpecialFocus,
                                      TenantId = ucf.TenantId,
                                      UserId = ucf.UserId,
                                      UserName = ucc.UserName,
                                      Online = ucc.State == UserOnlineState.Online,
                                  };

            return await userFriendQuery
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetMembersCountAsync(Guid userId, string filter = "", CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userChatCardQuery = dbContext.Set<UserChatCard>()
                 .WhereIf(!filter.IsNullOrWhiteSpace(), ucc => ucc.UserName.Contains(filter) || ucc.NickName.Contains(filter));

            var userChatFriendQuery = dbContext.Set<UserChatFriend>()
                .Where(ucf => ucf.Status == UserFriendStatus.Added)
                .WhereIf(!filter.IsNullOrWhiteSpace(), ucf => ucf.RemarkName.Contains(filter));

            var userFriendQuery = from ucf in userChatFriendQuery
                                  join ucc in userChatCardQuery
                                        on ucf.FrientId equals ucc.UserId
                                  where ucf.UserId == userId
                                  select ucc;

            return await userFriendQuery
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsFriendAsync(
            Guid userId,
            Guid frientId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .AnyAsync(ucf => ucf.UserId == userId && ucf.FrientId == frientId && ucf.Status == UserFriendStatus.Added,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsAddedAsync(Guid userId, Guid frientId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .AnyAsync(ucf => ucf.UserId == userId && ucf.FrientId == frientId,
                    GetCancellationToken(cancellationToken));
        }
    }
}

using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.Group;
using Microsoft.EntityFrameworkCore;
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
    public class EfCoreMessageRepository : EfCoreRepository<IMessageServiceDbContext, Message, long>,
        IMessageRepository, ITransientDependency
    {
        public EfCoreMessageRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<GroupMessage> GetGroupMessageAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<GroupMessage>()
                .Where(x => x.MessageId.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<GroupMessage>> GetGroupMessagesAsync(
            long groupId, 
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var groupMessages = await (await GetDbContextAsync()).Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .PageBy(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return groupMessages;
        }

        public virtual async Task<long> GetGroupMessagesCountAsync(
            long groupId,
            string filter = "", 
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            var groupMessagesCount = await (await GetDbContextAsync()).Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
            return groupMessagesCount;
        }

        public virtual async Task<List<GroupMessage>> GetUserGroupMessagesAsync(
            Guid sendUserId, 
            long groupId,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            MessageType? type = null,
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var groupMessages = await (await GetDbContextAsync()).Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .PageBy(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return groupMessages;
        }

        public virtual async Task<long> GetUserGroupMessagesCountAsync(
            Guid sendUserId, 
            long groupId, 
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            var groupMessagesCount = await (await GetDbContextAsync()).Set<GroupMessage>()
                  .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
                  .WhereIf(type != null, x => x.Type.Equals(type))
                  .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                  .LongCountAsync(GetCancellationToken(cancellationToken));
            return groupMessagesCount;
        }

        public virtual async Task<long> GetCountAsync(
            long groupId,
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<GroupMessage>()
                .Where(x => x.GroupId.Equals(groupId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(
            Guid sendUserId,
            Guid receiveUserId,
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                             x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<UserMessage> GetUserMessageAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => x.MessageId.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<LastChatMessage>> GetLastMessagesByOneFriendAsync(
            Guid userId,
            string sorting = nameof(LastChatMessage.SendTime),
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var groupMsgQuery = dbContext.Set<UserMessage>()
                .Where(msg => msg.ReceiveUserId == userId || msg.CreatorId == userId)
                .GroupBy(msg => new { msg.CreatorId, msg.ReceiveUserId })
                .Select(msg => new
                {
                    msg.Key.CreatorId,
                    msg.Key.ReceiveUserId,
                    MessageId = msg.Max(x => x.MessageId)
                });

            var userMessageQuery = from msg in dbContext.Set<UserMessage>()
                                   join gMsg in groupMsgQuery
                                        on msg.MessageId equals gMsg.MessageId
                                   select new LastChatMessage
                                   {
                                       Content = msg.Content,
                                       SendTime = msg.CreationTime,
                                       FormUserId = msg.CreatorId.Value,
                                       FormUserName = msg.SendUserName,
                                       MessageId = msg.MessageId.ToString(),
                                       MessageType = msg.Type,
                                       TenantId = msg.TenantId,
                                       ToUserId = msg.ReceiveUserId
                                   };

            return await userMessageQuery
                .OrderBy(sorting ?? nameof(LastChatMessage.SendTime))
                .Take(maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserMessage>> GetUserMessagesAsync(
            Guid sendUserId,
            Guid receiveUserId,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var userMessages = await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                             x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(UserMessage.MessageId))
                .PageBy(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return userMessages;
        }

        public virtual async Task<long> GetUserMessagesCountAsync(
            Guid sendUserId, 
            Guid receiveUserId,
            string filter = "", 
            MessageType? type = null,
            CancellationToken cancellationToken = default)
        {
            var userMessagesCount = await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                             x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));

            return userMessagesCount;
        }

        public virtual async Task InsertGroupMessageAsync(
            GroupMessage groupMessage,
            CancellationToken cancellationToken = default)
        {
            await (await GetDbContextAsync()).Set<GroupMessage>()
                .AddAsync(groupMessage, GetCancellationToken(cancellationToken));
        }

        public virtual async Task UpdateGroupMessageAsync(
            GroupMessage groupMessage,
            CancellationToken cancellationToken = default)
        {
            (await GetDbContextAsync()).Set<GroupMessage>().Update(groupMessage);
        }

        public virtual async Task InsertUserMessageAsync(
            UserMessage userMessage,
            CancellationToken cancellationToken = default)
        {
            await (await GetDbContextAsync()).Set<UserMessage>()
                .AddAsync(userMessage, GetCancellationToken(cancellationToken));
        }

        public virtual async Task UpdateUserMessageAsync(
            UserMessage userMessage,
            CancellationToken cancellationToken = default)
        {
            (await GetDbContextAsync()).Set<UserMessage>().Update(userMessage);
        }
    }
}

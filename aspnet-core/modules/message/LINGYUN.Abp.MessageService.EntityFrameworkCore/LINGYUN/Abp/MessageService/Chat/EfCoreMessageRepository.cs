using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreMessageRepository : EfCoreRepository<MessageServiceDbContext, Message, long>,
        IMessageRepository, ITransientDependency
    {
        public EfCoreMessageRepository(
            IDbContextProvider<MessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<GroupMessage> GetGroupMessageAsync(long id)
        {
            return await DbContext.Set<GroupMessage>()
                .Where(x => x.MessageId.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<GroupMessage>> GetGroupMessagesAsync(long groupId, string filter = "",
            string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10)
        {
            var groupMessages = await DbContext.Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .Page(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync();

            return groupMessages;
        }

        public async Task<long> GetGroupMessagesCountAsync(long groupId, string filter = "", MessageType? type = null)
        {
            var groupMessagesCount = await DbContext.Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync();
            return groupMessagesCount;
        }

        public async Task<List<GroupMessage>> GetUserGroupMessagesAsync(Guid sendUserId, long groupId, string filter = "",
            string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10)
        {
            var groupMessages = await DbContext.Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .Page(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync();

            return groupMessages;
        }

        public async Task<long> GetUserGroupMessagesCountAsync(Guid sendUserId, long groupId, string filter = "", MessageType? type = null)
        {
            var groupMessagesCount = await DbContext.Set<GroupMessage>()
                  .Distinct()
                  .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
                  .WhereIf(type != null, x => x.Type.Equals(type))
                  .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                  .LongCountAsync();
            return groupMessagesCount;
        }

        public async Task<UserMessage> GetUserMessageAsync(long id)
        {
            return await DbContext.Set<UserMessage>()
                .Where(x => x.MessageId.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserMessage>> GetUserMessagesAsync(Guid sendUserId, Guid receiveUserId, string filter = "",
            string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10)
        {
            var userMessages = await DbContext.Set<UserMessage>()
                .Distinct()
                .Where(x => x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .Page(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync();

            return userMessages;
        }

        public async Task<long> GetUserMessagesCountAsync(Guid sendUserId, Guid receiveUserId, string filter = "", MessageType? type = null)
        {
            var userMessagesCount = await DbContext.Set<UserMessage>()
                .Distinct()
                .Where(x => x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId))
                .WhereIf(type != null, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .LongCountAsync();

            return userMessagesCount;
        }

        public async Task<GroupMessage> InsertGroupMessageAsync(GroupMessage groupMessage, bool saveChangs = false)
        {
            groupMessage = (await DbContext.Set<GroupMessage>().AddAsync(groupMessage, GetCancellationToken())).Entity;

            if(saveChangs)
            {
                await DbContext.SaveChangesAsync();
            }

            return groupMessage;
        }

        public async Task<UserMessage> InsertUserMessageAsync(UserMessage userMessage, bool saveChangs = false)
        {
            userMessage = (await DbContext.Set<UserMessage>().AddAsync(userMessage, GetCancellationToken())).Entity;

            if (saveChangs)
            {
                await DbContext.SaveChangesAsync();
            }

            return userMessage;
        }
    }
}

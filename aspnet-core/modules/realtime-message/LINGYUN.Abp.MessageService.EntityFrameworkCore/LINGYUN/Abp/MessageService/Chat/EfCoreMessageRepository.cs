using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace LINGYUN.Abp.MessageService.Chat;

public class EfCoreMessageRepository : EfCoreRepository<IMessageServiceDbContext, Message, long>,
    IMessageRepository, ITransientDependency
{
    public EfCoreMessageRepository(
        IDbContextProvider<IMessageServiceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<GroupMessage> GetGroupMessageAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<GroupMessage>()
            .Where(x => x.MessageId.Equals(id))
            .AsNoTracking()
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<GroupMessage>> GetGroupMessagesAsync(
        long groupId,
        MessageType? type = null,
        string filter = "",
        string sorting = nameof(UserMessage.MessageId),
        int skipCount = 0,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(GroupMessage.MessageId);
        }
        var groupMessages = await (await GetDbContextAsync()).Set<GroupMessage>()
            .Distinct()
            .Where(x => x.GroupId.Equals(groupId))
            .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
            .WhereIf(type.HasValue, x => x.Type.Equals(type))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));

        return groupMessages;
    }

    public async virtual Task<long> GetGroupMessagesCountAsync(
        long groupId,
        MessageType? type = null,
        string filter = "",
        CancellationToken cancellationToken = default)
    {
        var groupMessagesCount = await (await GetDbContextAsync()).Set<GroupMessage>()
            .Distinct()
            .Where(x => x.GroupId.Equals(groupId))
            .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
            .WhereIf(type.HasValue, x => x.Type.Equals(type))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
        return groupMessagesCount;
    }

    public async virtual Task<List<GroupMessage>> GetUserGroupMessagesAsync(
        Guid sendUserId,
        long groupId,
        MessageType? type = null,
        string filter = "",
        string sorting = nameof(UserMessage.MessageId),
        int skipCount = 0,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(GroupMessage.MessageId);
        }
        var groupMessages = await (await GetDbContextAsync()).Set<GroupMessage>()
            .Distinct()
            .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
            .WhereIf(type != null, x => x.Type.Equals(type))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));

        return groupMessages;
    }

    public async virtual Task<long> GetUserGroupMessagesCountAsync(
        Guid sendUserId,
        long groupId,
        MessageType? type = null,
        string filter = "",
        CancellationToken cancellationToken = default)
    {
        var groupMessagesCount = await (await GetDbContextAsync()).Set<GroupMessage>()
              .Where(x => x.GroupId.Equals(groupId) && x.CreatorId.Equals(sendUserId))
              .WhereIf(type != null, x => x.Type.Equals(type))
              .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
              .LongCountAsync(GetCancellationToken(cancellationToken));
        return groupMessagesCount;
    }

    public async virtual Task<long> GetCountAsync(
        long groupId,
        MessageType? type = null,
        string filter = "",
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<GroupMessage>()
            .Where(x => x.GroupId.Equals(groupId))
            .WhereIf(type != null, x => x.Type.Equals(type))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<long> GetCountAsync(
        Guid sendUserId,
        Guid receiveUserId,
        MessageType? type = null,
        string filter = "",
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<UserMessage>()
            .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                         x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
            .WhereIf(type != null, x => x.Type.Equals(type))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<UserMessage> GetUserMessageAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<UserMessage>()
            .Where(x => x.MessageId.Equals(id))
            .AsNoTracking()
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<LastChatMessage>> GetLastMessagesAsync(
        Guid userId,
        MessageState? state = null,
        string sorting = nameof(LastChatMessage.SendTime),
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        // 参数验证和默认值设置
        sorting = sorting.IsNullOrWhiteSpace() ? $"{nameof(LastChatMessage.SendTime)} DESC" : sorting;
        var dbContext = await GetDbContextAsync();
        var token = GetCancellationToken(cancellationToken);

        // 使用Union All合并用户消息和群组消息查询
        var lastMessages = await (
            // 用户消息查询
            from um in dbContext.Set<UserMessage>()
            join ucc in dbContext.Set<UserChatCard>()
                on um.CreatorId equals ucc.UserId
            where um.ReceiveUserId == userId &&
                  (state == null || um.State == state) &&
                  um.MessageId == (
                      from subUm in dbContext.Set<UserMessage>()
                      where subUm.ReceiveUserId == userId &&
                            subUm.CreatorId == um.CreatorId
                      select subUm.MessageId
                  ).Max()
            select new LastChatMessage
            {
                Content = um.Content,
                SendTime = um.CreationTime,
                FormUserId = um.CreatorId.Value,
                FormUserName = um.SendUserName,
                Object = ucc.NickName,
                AvatarUrl = ucc.AvatarUrl,
                Source = um.Source,
                MessageId = um.MessageId.ToString(),
                MessageType = um.Type,
                TenantId = um.TenantId,
                ToUserId = um.ReceiveUserId.ToString(),
                GroupId = ""
            })
            .Union(
                // 群组消息查询
                from gm in dbContext.Set<GroupMessage>()
                join cg in dbContext.Set<ChatGroup>()
                    on gm.GroupId equals cg.GroupId
                join ucg in dbContext.Set<UserChatGroup>()
                    on new { GroupId = gm.GroupId, UserId = userId } equals new { ucg.GroupId, ucg.UserId }
                where (state == null || gm.State == state) &&
                      gm.MessageId == (
                          from subGm in dbContext.Set<GroupMessage>()
                          where subGm.GroupId == gm.GroupId
                          select subGm.MessageId
                      ).Max()
                select new LastChatMessage
                {
                    Content = gm.Content,
                    SendTime = gm.CreationTime,
                    FormUserId = gm.CreatorId.Value,
                    FormUserName = gm.SendUserName,
                    Object = cg.Name,
                    AvatarUrl = cg.AvatarUrl,
                    Source = gm.Source,
                    MessageId = gm.MessageId.ToString(),
                    MessageType = gm.Type,
                    TenantId = gm.TenantId,
                    ToUserId = "",
                    GroupId = gm.GroupId.ToString()
                }
            )
            // 排序和分页
            .OrderBy(sorting)
            .Take(maxResultCount)
            .ToListAsync(token);

        return lastMessages;
    }

    public async virtual Task<List<UserMessage>> GetUserMessagesAsync(
        Guid sendUserId,
        Guid receiveUserId,
        MessageType? type = null,
        string filter = "",
        string sorting = nameof(UserMessage.MessageId),
        int skipCount = 0,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(UserMessage.MessageId);
        }
        var userMessages = await (await GetDbContextAsync()).Set<UserMessage>()
            .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                         x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
            .WhereIf(type.HasValue, x => x.Type.Equals(type))
            .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));

        return userMessages;
    }

    public async virtual Task<long> GetUserMessagesCountAsync(
        Guid sendUserId,
        Guid receiveUserId,
        MessageType? type = null,
        string filter = "",
        CancellationToken cancellationToken = default)
    {
        var userMessagesCount = await (await GetDbContextAsync()).Set<UserMessage>()
            .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                         x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
            .WhereIf(type.HasValue, x => x.Type.Equals(type))
            .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));

        return userMessagesCount;
    }

    public async virtual Task InsertGroupMessageAsync(
        GroupMessage groupMessage,
        CancellationToken cancellationToken = default)
    {
        await (await GetDbContextAsync()).Set<GroupMessage>()
            .AddAsync(groupMessage, GetCancellationToken(cancellationToken));
    }

    public async virtual Task UpdateGroupMessageAsync(
        GroupMessage groupMessage,
        CancellationToken cancellationToken = default)
    {
        (await GetDbContextAsync()).Set<GroupMessage>().Update(groupMessage);
    }

    public async virtual Task InsertUserMessageAsync(
        UserMessage userMessage,
        CancellationToken cancellationToken = default)
    {
        await (await GetDbContextAsync()).Set<UserMessage>()
            .AddAsync(userMessage, GetCancellationToken(cancellationToken));
    }

    public async virtual Task UpdateUserMessageAsync(
        UserMessage userMessage,
        CancellationToken cancellationToken = default)
    {
        (await GetDbContextAsync()).Set<UserMessage>().Update(userMessage);
    }
}

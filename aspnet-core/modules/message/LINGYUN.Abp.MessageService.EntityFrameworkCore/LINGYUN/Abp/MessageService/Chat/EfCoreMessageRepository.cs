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
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var groupMessages = await (await GetDbContextAsync()).Set<GroupMessage>()
                .Distinct()
                .Where(x => x.GroupId.Equals(groupId))
                .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
                .WhereIf(type.HasValue, x => x.Type.Equals(type))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Content.Contains(filter) || x.SendUserName.Contains(filter))
                .OrderBy(sorting ?? nameof(GroupMessage.MessageId))
                .PageBy(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return groupMessages;
        }

        public virtual async Task<long> GetGroupMessagesCountAsync(
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

        public virtual async Task<List<GroupMessage>> GetUserGroupMessagesAsync(
            Guid sendUserId,
            long groupId,
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
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

        public virtual async Task<long> GetCountAsync(
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

        public virtual async Task<long> GetCountAsync(
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

        public virtual async Task<UserMessage> GetUserMessageAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => x.MessageId.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<LastChatMessage>> GetLastMessagesAsync(
            Guid userId,
            MessageState? state = null,
            string sorting = nameof(LastChatMessage.SendTime),
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            sorting ??= $"{nameof(LastChatMessage.SendTime)} DESC";
            var dbContext = await GetDbContextAsync();

            #region SQL 原型

            var sqlBuilder = new StringBuilder(1280);
            sqlBuilder.AppendLine("SELECT");
            sqlBuilder.AppendLine("    msg.* ");
            sqlBuilder.AppendLine("FROM");
            sqlBuilder.AppendLine("    (");
            sqlBuilder.AppendLine("    SELECT");
            sqlBuilder.AppendLine("        um.Content,");
            sqlBuilder.AppendLine("        um.CreationTime,");
            sqlBuilder.AppendLine("        um.CreatorId,");
            sqlBuilder.AppendLine("        um.SendUserName,");
            sqlBuilder.AppendLine("        ac.NickName AS Object,");
            sqlBuilder.AppendLine("        ac.AvatarUrl,");
            sqlBuilder.AppendLine("        um.Source,");
            sqlBuilder.AppendLine("        um.MessageId,");
            sqlBuilder.AppendLine("        um.Type,");
            sqlBuilder.AppendLine("        um.TenantId,");
            sqlBuilder.AppendLine("        um.ReceiveUserId,");
            sqlBuilder.AppendLine("        '' AS GroupId,");
            sqlBuilder.AppendLine("        um.ExtraProperties");
            sqlBuilder.AppendLine("    FROM");
            sqlBuilder.AppendLine("        (");
            sqlBuilder.AppendLine("        SELECT");
            sqlBuilder.AppendLine("            um.* ");
            sqlBuilder.AppendLine("        FROM");
            sqlBuilder.AppendLine("            appusermessages um");
            sqlBuilder.AppendLine("            INNER JOIN ( SELECT max( um.MessageId ) AS MessageId FROM appusermessages um");
            sqlBuilder.AppendLine("            WHERE");
            sqlBuilder.AppendLine("              um.ReceiveUserId = @ReceiveUserId");
            if (state.HasValue)
            {
                sqlBuilder.AppendLine("              AND um.State = @State");
            }
            if (CurrentTenant.IsAvailable)
            {
                sqlBuilder.AppendLine("              AND um.TenantId = @TenantId");
            }
            sqlBuilder.AppendLine("            GROUP BY um.ReceiveUserId ) gum ON um.MessageId = gum.MessageId");
            sqlBuilder.AppendLine("        ) um");
            sqlBuilder.AppendLine("        LEFT JOIN appuserchatcards ac ON ac.UserId = um.CreatorId ");
            sqlBuilder.AppendLine("    UNION ALL");
            sqlBuilder.AppendLine("    SELECT");
            sqlBuilder.AppendLine("        gm.Content,");
            sqlBuilder.AppendLine("        gm.CreationTime,");
            sqlBuilder.AppendLine("        gm.CreatorId,");
            sqlBuilder.AppendLine("        gm.SendUserName,");
            sqlBuilder.AppendLine("        ag.Name AS Object,");
            sqlBuilder.AppendLine("        ag.AvatarUrl,");
            sqlBuilder.AppendLine("        gm.Source,");
            sqlBuilder.AppendLine("        gm.MessageId,");
            sqlBuilder.AppendLine("        gm.Type,");
            sqlBuilder.AppendLine("        gm.TenantId,");
            sqlBuilder.AppendLine("        '' AS ReceiveUserId,");
            sqlBuilder.AppendLine("        gm.GroupId,");
            sqlBuilder.AppendLine("        gm.ExtraProperties");
            sqlBuilder.AppendLine("    FROM");
            sqlBuilder.AppendLine("        appgroupmessages gm");
            sqlBuilder.AppendLine("        INNER JOIN (");
            sqlBuilder.AppendLine("        SELECT");
            sqlBuilder.AppendLine("          max( gm.MessageId ) AS MessageId ");
            sqlBuilder.AppendLine("        FROM");
            sqlBuilder.AppendLine("          appuserchatcards ac");
            sqlBuilder.AppendLine("          LEFT JOIN appuserchatgroups acg ON acg.UserId = ac.UserId");
            sqlBuilder.AppendLine("          LEFT JOIN appgroupmessages gm ON gm.GroupId = acg.GroupId ");
            sqlBuilder.AppendLine("        WHERE");
            sqlBuilder.AppendLine("          ac.UserId = @ReceiveUserId ");
            if (state.HasValue)
            {
                sqlBuilder.AppendLine("          AND gm.State = @State");
            }
            if (CurrentTenant.IsAvailable)
            {
                sqlBuilder.AppendLine("          AND gm.TenantId = @TenantId");
            }
            sqlBuilder.AppendLine("        GROUP BY");
            sqlBuilder.AppendLine("        gm.GroupId");
            sqlBuilder.AppendLine("        ) ggm ON ggm.MessageId = gm.MessageId ");
            sqlBuilder.AppendLine("        INNER JOIN appchatgroups ag on ag.GroupId = gm.GroupId");
            sqlBuilder.AppendLine("    ) AS msg");
            sqlBuilder.AppendLine("ORDER BY ");
            sqlBuilder.AppendLine("   @Sorting");
            sqlBuilder.AppendLine("   LIMIT @MaxResultCount");

            using var dbContection = dbContext.Database.GetDbConnection();
            await dbContection.OpenAsync();
            using var command = dbContection.CreateCommand();
            command.Transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandText = sqlBuilder.ToString();

            var receivedUser = command.CreateParameter();
            receivedUser.DbType = System.Data.DbType.Guid;
            receivedUser.ParameterName = "@ReceiveUserId";
            receivedUser.Value = userId;
            command.Parameters.Add(receivedUser);

            var sorttingParam = command.CreateParameter();
            sorttingParam.DbType = System.Data.DbType.String;
            sorttingParam.ParameterName = "@Sorting";
            sorttingParam.Value = sorting;
            command.Parameters.Add(sorttingParam);

            var limitParam = command.CreateParameter();
            limitParam.DbType = System.Data.DbType.Int32;
            limitParam.ParameterName = "@MaxResultCount";
            limitParam.Value = maxResultCount;
            command.Parameters.Add(limitParam);

            if (state.HasValue)
            {
                var stateParam = command.CreateParameter();
                stateParam.DbType = System.Data.DbType.Int32;
                stateParam.ParameterName = "@State";
                stateParam.Value = (int)state.Value;
                command.Parameters.Add(stateParam);
            }
            if (CurrentTenant.IsAvailable)
            {
                var tenantParam = command.CreateParameter();
                tenantParam.DbType = System.Data.DbType.Guid;
                tenantParam.ParameterName = "@TenantId";
                tenantParam.Value = CurrentTenant.Id.Value;
                command.Parameters.Add(tenantParam);
            }
            var messages = new List<LastChatMessage>();
            using var reader = await command.ExecuteReaderAsync();

            T GetValue<T>(DbDataReader reader, int index)
            {
                var value = reader.GetValue(index);
                if (value == null || value == DBNull.Value)
                {
                    return default;
                }

                var valueType = typeof(T);
                var converter = TypeDescriptor.GetConverter(valueType);
                if (converter.CanConvertFrom(value.GetType()))
                {
                    return (T)converter.ConvertFrom(value);
                }
                return (T)Convert.ChangeType(value, typeof(T));
            };

            ExtraPropertyDictionary GetExtraProperties(DbDataReader reader, int index)
            {
                var value = reader.GetValue(index);
                if (value == null || value == DBNull.Value)
                {
                    return new ExtraPropertyDictionary();
                }
                var extraPropertiesAsJson = value.ToString();
                if (extraPropertiesAsJson.IsNullOrEmpty() || extraPropertiesAsJson == "{}")
                {
                    return new ExtraPropertyDictionary();
                }

                var deserializeOptions = new JsonSerializerOptions();
                deserializeOptions.Converters.Add(new ObjectToInferredTypesConverter());

                var dictionary = JsonSerializer.Deserialize<ExtraPropertyDictionary>(extraPropertiesAsJson, deserializeOptions) ??
                                 new ExtraPropertyDictionary();

                return dictionary;
            }

            while (reader.Read())
            {
                messages.Add(new LastChatMessage
                {
                    Content = GetValue<string>(reader, 0),
                    SendTime = GetValue<DateTime>(reader, 1),
                    FormUserId = GetValue<Guid>(reader, 2),
                    FormUserName = GetValue<string>(reader, 3),
                    Object = GetValue<string>(reader, 4),
                    AvatarUrl = GetValue<string>(reader, 5),
                    Source = (MessageSourceTye)GetValue<int>(reader, 6),
                    MessageId = GetValue<string>(reader, 7),
                    MessageType = (MessageType)GetValue<int>(reader, 8),
                    TenantId = GetValue<Guid?>(reader, 9),
                    ToUserId = GetValue<string>(reader, 10),
                    GroupId = GetValue<string>(reader, 11),
                    ExtraProperties = GetExtraProperties(reader, 12),
                });
            }

            return messages;
            #endregion

            #region 待 EF 团队解决此问题

            //// 聚合用户消息
            //var aggreUserMsgIdQuery = dbContext.Set<UserMessage>()
            //    .Where(msg => msg.ReceiveUserId == userId)
            //    .WhereIf(state.HasValue, x => x.SendState == state)
            //    .GroupBy(msg => msg.ReceiveUserId)
            //    .Select(msg => new
            //    {
            //        MessageId = msg.Max(x => x.MessageId)
            //    });
            //var joinUserMsg = from um in dbContext.Set<UserMessage>()
            //                  join aum in aggreUserMsgIdQuery
            //                       on um.MessageId equals aum.MessageId
            //                  join ucc in dbContext.Set<UserChatCard>()
            //                        on um.CreatorId equals ucc.UserId
            //                  select new LastChatMessage
            //                  {
            //                      Content = um.Content,
            //                      SendTime = um.CreationTime,
            //                      FormUserId = um.CreatorId.Value,
            //                      FormUserName = um.SendUserName,
            //                      Object = ucc.NickName,
            //                      AvatarUrl = ucc.AvatarUrl,
            //                      Source = um.Source,
            //                      MessageId = Convert.ToString(um.MessageId),
            //                      MessageType = um.Type,
            //                      TenantId = um.TenantId,
            //                      // ToUserId = Convert.ToString(um.ReceiveUserId),
            //                      // GroupId = "",
            //                  };
            //// 聚合群组消息
            //var aggreGroupMsgIdQuery = from ucc in dbContext.Set<UserChatCard>()
            //                           join ucg in dbContext.Set<UserChatGroup>()
            //                                on ucc.UserId equals ucg.UserId
            //                           join gm in dbContext.Set<GroupMessage>()
            //                                on ucg.GroupId equals gm.GroupId
            //                           where ucc.UserId.Equals(userId)
            //                           group gm by gm.GroupId into ggm
            //                           select new
            //                           {
            //                               MessageId = ggm.Max(gm => gm.MessageId),
            //                           };

            //var joinGroupMsg = from gm in dbContext.Set<GroupMessage>()
            //                   join agm in aggreGroupMsgIdQuery
            //                        on gm.MessageId equals agm.MessageId
            //                   join cg in dbContext.Set<ChatGroup>()
            //                        on gm.GroupId equals cg.GroupId
            //                   select new LastChatMessage
            //                   {
            //                       Content = gm.Content,
            //                       SendTime = gm.CreationTime,
            //                       FormUserId = gm.CreatorId.Value,
            //                       FormUserName = gm.SendUserName,
            //                       Object = cg.Name,
            //                       AvatarUrl = cg.AvatarUrl,
            //                       Source = gm.Source,
            //                       MessageId = Convert.ToString(gm.MessageId),
            //                       MessageType = gm.Type,
            //                       TenantId = gm.TenantId,
            //                       // ToUserId = "",
            //                       // GroupId = Convert.ToString(gm.GroupId)
            //                   };

            //return await joinUserMsg
            //    .Concat(joinGroupMsg)
            //    .OrderBy(sorting)
            //    .Take(maxResultCount)
            //    .ToListAsync(GetCancellationToken(cancellationToken));

            #endregion

        }

        public virtual async Task<List<UserMessage>> GetUserMessagesAsync(
            Guid sendUserId,
            Guid receiveUserId,
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var userMessages = await (await GetDbContextAsync()).Set<UserMessage>()
                .Where(x => (x.CreatorId.Equals(sendUserId) && x.ReceiveUserId.Equals(receiveUserId)) ||
                             x.CreatorId.Equals(receiveUserId) && x.ReceiveUserId.Equals(sendUserId))
                .WhereIf(type.HasValue, x => x.Type.Equals(type))
                .Where(x => x.State == MessageState.Send || x.State == MessageState.Read)
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

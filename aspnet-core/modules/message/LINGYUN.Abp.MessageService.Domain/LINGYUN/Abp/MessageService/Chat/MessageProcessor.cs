using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.MessageService.Chat
{
    [Dependency(ReplaceServices = true)]
    public class MessageProcessor : IMessageProcessor, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly IMessageRepository _repository;
        private readonly ISettingProvider _settingProvider;

        public MessageProcessor(
            IClock clock,
            IMessageRepository repository,
            ISettingProvider settingProvider)
        {
            _clock = clock;
            _repository = repository;
            _settingProvider = settingProvider;
        }

        public virtual async Task ReadAsync(ChatMessage message)
        {
            if (!message.GroupId.IsNullOrWhiteSpace())
            {
                long messageId = long.Parse(message.MessageId);
                var groupMessage = await _repository.GetGroupMessageAsync(messageId);
                groupMessage.ChangeSendState(MessageState.Read);

                await _repository.UpdateGroupMessageAsync(groupMessage);
            }
            else
            {
                long messageId = long.Parse(message.MessageId);
                var userMessage = await _repository.GetUserMessageAsync(messageId);
                userMessage.ChangeSendState(MessageState.Read);

                await _repository.UpdateUserMessageAsync(userMessage);
            }
        }

        public virtual async Task ReCallAsync(ChatMessage message)
        {
            var expiration = await _settingProvider.GetAsync(
                MessageServiceSettingNames.Messages.RecallExpirationTime, 2d);

            Func<Message, bool> hasExpiredMessage = (Message msg) =>
                msg.CreationTime.AddMinutes(expiration) < _clock.Now;

            if (!message.GroupId.IsNullOrWhiteSpace())
            {
                long messageId = long.Parse(message.MessageId);
                var groupMessage = await _repository.GetGroupMessageAsync(messageId);
                if (hasExpiredMessage(groupMessage))
                {
                    throw new BusinessException(MessageServiceErrorCodes.ExpiredMessageCannotBeReCall)
                        .WithData("Time", expiration);
                }

                groupMessage.ChangeSendState(MessageState.ReCall);

                await _repository.UpdateGroupMessageAsync(groupMessage);
            }
            else
            {
                long messageId = long.Parse(message.MessageId);
                var userMessage = await _repository.GetUserMessageAsync(messageId);
                if (hasExpiredMessage(userMessage))
                {
                    throw new BusinessException(MessageServiceErrorCodes.ExpiredMessageCannotBeReCall)
                        .WithData("Time", expiration);
                }

                userMessage.ChangeSendState(MessageState.ReCall);

                await _repository.UpdateUserMessageAsync(userMessage);
            }
        }
    }
}

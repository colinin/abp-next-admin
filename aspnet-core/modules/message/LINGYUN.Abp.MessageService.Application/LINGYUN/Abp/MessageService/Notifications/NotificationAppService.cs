using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [Authorize]
    public class NotificationAppService : ApplicationService, INotificationAppService
    {
        protected ITemplateContentProvider TemplateContentProvider { get; }
        protected INotificationSender NotificationSender { get; }
        protected INotificationDefinitionManager NotificationDefinitionManager { get; }
        protected INotificationTemplateRepository NotificationTemplateRepository { get; }

        public NotificationAppService(
            INotificationSender notificationSender,
            ITemplateContentProvider templateContentProvider,
            INotificationDefinitionManager notificationDefinitionManager,
            INotificationTemplateRepository notificationTemplateRepository)
        {
            NotificationSender = notificationSender;
            TemplateContentProvider = templateContentProvider;
            NotificationDefinitionManager = notificationDefinitionManager;
            NotificationTemplateRepository = notificationTemplateRepository;
        }

        public async virtual Task<NotificationTemplateDto> SetTemplateAsync(NotificationTemplateSetInput input)
        {
            var notification = GetNotificationDefinition(input.Name);

            var template = await NotificationTemplateRepository.GetByNameAsync(input.Name, input.Culture);
            if (template == null)
            {
                template = new NotificationTemplate(
                    GuidGenerator.Create(),
                    notification.Name,
                    notification.DisplayName.Localize(StringLocalizerFactory),
                    input.Content,
                    input.Culture,
                    notification.Description?.Localize(StringLocalizerFactory));

                template = await NotificationTemplateRepository.InsertAsync(template);
            }
            else
            {
                template.SetContent(input.Content);

                await NotificationTemplateRepository.UpdateAsync(template);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<NotificationTemplate, NotificationTemplateDto>(template);
        }

        public async virtual Task<NotificationTemplateDto> GetTemplateAsync(NotificationTemplateGetInput input)
        {
            var notification = GetNotificationDefinition(input.Name);

            var culture = input.Culture ?? CultureInfo.CurrentCulture.Name;
            var content = await TemplateContentProvider.GetContentOrNullAsync(notification.Name, culture);

            return new NotificationTemplateDto
            {
                Culture = culture,
                Content = content,
                Name = notification.Name,
                Title = notification.DisplayName.Localize(StringLocalizerFactory),
                Description = notification.Description?.Localize(StringLocalizerFactory),
            };
        }

        public virtual Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync()
        {
            var templates = new List<NotificationTemplateDto>();
            var notifications = NotificationDefinitionManager.GetAll().Where(n => n.Template != null);

            foreach (var notification in notifications)
            {
                templates.Add(
                    new NotificationTemplateDto
                    {
                        Name = notification.Name,
                        Culture = CultureInfo.CurrentCulture.Name,
                        Title = notification.DisplayName.Localize(StringLocalizerFactory),
                        Description = notification.Description?.Localize(StringLocalizerFactory),
                    });
            }

            return Task.FromResult(new ListResultDto<NotificationTemplateDto>(templates));
        }

        public async virtual Task SendAsync(NotificationSendDto input)
        {
            var notification = GetNotificationDefinition(input.Name);

            UserIdentifier user = null;
            if (input.ToUserId.HasValue)
            {
                user = new UserIdentifier(input.ToUserId.Value, input.ToUserName);
            }

            await NotificationSender
                .SendNofiterAsync(
                    name: input.Name,
                    template: new Abp.Notifications.NotificationTemplate(
                        notification.Name,
                        culture: input.Culture ?? CultureInfo.CurrentCulture.Name,
                        formUser: CurrentUser.Name ?? CurrentUser.UserName,
                        data: input.Data),
                    user: user,
                    CurrentTenant.Id,
                    input.Severity);
        }

        protected virtual NotificationDefinition GetNotificationDefinition(string name)
        {
            var notification = NotificationDefinitionManager.GetOrNull(name);
            if (notification == null || notification.Template == null)
            {
                throw new BusinessException(
                    MessageServiceErrorCodes.NotificationTemplateNotFound,
                    $"The notification template {name} does not exist!")
                    .WithData("Name", name);
            }

            return notification;
        }
    }
}

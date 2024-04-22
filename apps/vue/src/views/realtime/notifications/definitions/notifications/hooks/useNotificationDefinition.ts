import { computed, ref, unref, onMounted } from 'vue';
import {
  NotificationLifetime,
  NotificationType,
  NotificationContentType,
  NotificationSeverity,
} from '/@/api/realtime/notifications/types';
import { GetListAsyncByInput as getAvailableTemplates } from '/@/api/text-templating/definitions';
import { TextTemplateDefinitionDto } from '/@/api/text-templating/definitions/model';
import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
import { useLocalization } from '/@/hooks/abp/useLocalization';

export function useNotificationDefinition() {
  const availableTemplates = ref<TextTemplateDefinitionDto[]>([]);
  const { L, Lr } = useLocalization(['Notifications']);
  const { deserialize } = useLocalizationSerializer();

  const getAvailableTemplateOptions = computed(() => {
    const templates = unref(availableTemplates);
    return templates.map((item) => {
      return {
        label: item.displayName,
        value: item.name,
      };
    });
  });

  onMounted(fetchTemplates);

  function fetchTemplates() {
    getAvailableTemplates({}).then((res) => {
      formatDisplayName(res.items);
      availableTemplates.value = res.items;
    });
  }

  function formatDisplayName(list: any[]) {
    if (list && Array.isArray(list)) {
      list.forEach((item) => {
        if (Reflect.has(item, 'displayName')) {
          const info = deserialize(item.displayName);
          item.displayName = Lr(info.resourceName, info.name);
        }
        if (Reflect.has(item, 'children')) {
          formatDisplayName(item.children);
        }
      });
    }
  }

  const notificationLifetimeMap = {
    [NotificationLifetime.OnlyOne]: L('NotificationLifetime:OnlyOne'),
    [NotificationLifetime.Persistent]: L('NotificationLifetime:Persistent'),
  };
  const notificationLifetimeOptions = [
    { label: L('NotificationLifetime:OnlyOne'), value: NotificationLifetime.OnlyOne },
    { label: L('NotificationLifetime:Persistent'), value: NotificationLifetime.Persistent },
  ];

  const notificationTypeMap = {
    [NotificationType.User]: L('NotificationType:User'),
    [NotificationType.System]: L('NotificationType:System'),
    [NotificationType.Application]: L('NotificationType:Application'),
    [NotificationType.ServiceCallback]: L('NotificationType:ServiceCallback'),
  };
  const notificationTypeOptions = [
    { label: L('NotificationType:User'), value: NotificationType.User },
    { label: L('NotificationType:System'), value: NotificationType.System },
    { label: L('NotificationType:Application'), value: NotificationType.Application },
    { label: L('NotificationType:ServiceCallback'), value: NotificationType.ServiceCallback },
  ];

  const notificationContentTypeMap = {
    [NotificationContentType.Text]: L('NotificationContentType:Text'),
    [NotificationContentType.Json]: L('NotificationContentType:Json'),
    [NotificationContentType.Html]: L('NotificationContentType:Html'),
    [NotificationContentType.Markdown]: L('NotificationContentType:Markdown'),
  };
  const notificationContentTypeOptions = [
    { label: L('NotificationContentType:Text'), value: NotificationContentType.Text },
    { label: L('NotificationContentType:Json'), value: NotificationContentType.Json },
    { label: L('NotificationContentType:Html'), value: NotificationContentType.Html },
    { label: L('NotificationContentType:Markdown'), value: NotificationContentType.Markdown },
  ];

  const notificationSeverityOptions = [
    { label: L('NotificationSeverity:Success'), value: NotificationSeverity.Success },
    { label: L('NotificationSeverity:Info'), value: NotificationSeverity.Info },
    { label: L('NotificationSeverity:Warn'), value: NotificationSeverity.Warn },
    { label: L('NotificationSeverity:Fatal'), value: NotificationSeverity.Fatal },
    { label: L('NotificationSeverity:Error'), value: NotificationSeverity.Error },
  ];

  const notificationPushProviderOptions = [
    { label: L('Providers:Emailing'), value: 'Emailing' },
    { label: L('Providers:SignalR'), value: 'SignalR' },
    { label: L('Providers:WeChat.MiniProgram'), value: 'WeChat.MiniProgram' },
    { label: L('Providers:WeChat.Work'), value: 'WeChat.Work' },
    { label: L('Providers:Sms'), value: 'Sms' },
    { label: L('Providers:WxPusher'), value: 'WxPusher' },
  ];

  return {
    formatDisplayName,
    getAvailableTemplateOptions,
    notificationTypeMap,
    notificationLifetimeOptions,
    notificationLifetimeMap,
    notificationTypeOptions,
    notificationContentTypeMap,
    notificationContentTypeOptions,
    notificationPushProviderOptions,
    notificationSeverityOptions,
  }
}
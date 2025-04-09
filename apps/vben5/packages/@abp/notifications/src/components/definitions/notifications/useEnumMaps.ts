import { reactive } from 'vue';

import { $t } from '@vben/locales';

import {
  NotificationContentType,
  NotificationLifetime,
  NotificationSeverity,
  NotificationType,
} from '../../../types/notifications';

export function useEnumMaps() {
  const notificationTypeOptions = reactive([
    {
      label: $t('Notifications.NotificationType:User'),
      value: NotificationType.User,
    },
    {
      label: $t('Notifications.NotificationType:System'),
      value: NotificationType.System,
    },
    {
      label: $t('Notifications.NotificationType:Application'),
      value: NotificationType.Application,
    },
    {
      label: $t('Notifications.NotificationType:ServiceCallback'),
      value: NotificationType.ServiceCallback,
    },
  ]);
  const notificationTypeMap = reactive({
    [NotificationType.Application]: $t(
      'Notifications.NotificationType:Application',
    ),
    [NotificationType.ServiceCallback]: $t(
      'Notifications.NotificationType:ServiceCallback',
    ),
    [NotificationType.System]: $t('Notifications.NotificationType:System'),
    [NotificationType.User]: $t('Notifications.NotificationType:User'),
  });
  const notificationLifetimeOptions = reactive([
    {
      label: $t('Notifications.NotificationLifetime:OnlyOne'),
      value: NotificationLifetime.OnlyOne,
    },
    {
      label: $t('Notifications.NotificationLifetime:Persistent'),
      value: NotificationLifetime.Persistent,
    },
  ]);
  const notificationLifetimeMap = reactive({
    [NotificationLifetime.OnlyOne]: $t(
      'Notifications.NotificationLifetime:OnlyOne',
    ),
    [NotificationLifetime.Persistent]: $t(
      'Notifications.NotificationLifetime:Persistent',
    ),
  });
  const notificationContentTypeOptions = reactive([
    {
      label: $t('Notifications.NotificationContentType:Text'),
      value: NotificationContentType.Text,
    },
    {
      label: $t('Notifications.NotificationContentType:Json'),
      value: NotificationContentType.Json,
    },
    {
      label: $t('Notifications.NotificationContentType:Html'),
      value: NotificationContentType.Html,
    },
    {
      label: $t('Notifications.NotificationContentType:Markdown'),
      value: NotificationContentType.Markdown,
    },
  ]);
  const notificationContentTypeMap = reactive({
    [NotificationContentType.Html]: $t(
      'Notifications.NotificationContentType:Html',
    ),
    [NotificationContentType.Json]: $t(
      'Notifications.NotificationContentType:Json',
    ),
    [NotificationContentType.Markdown]: $t(
      'Notifications.NotificationContentType:Markdown',
    ),
    [NotificationContentType.Text]: $t(
      'Notifications.NotificationContentType:Text',
    ),
  });
  const notificationSeverityOptions = reactive([
    {
      label: $t('Notifications.NotificationSeverity:Success'),
      value: NotificationSeverity.Success,
    },
    {
      label: $t('Notifications.NotificationSeverity:Info'),
      value: NotificationSeverity.Info,
    },
    {
      label: $t('Notifications.NotificationSeverity:Warn'),
      value: NotificationSeverity.Warn,
    },
    {
      label: $t('Notifications.NotificationSeverity:Fatal'),
      value: NotificationSeverity.Fatal,
    },
    {
      label: $t('Notifications.NotificationSeverity:Error'),
      value: NotificationSeverity.Error,
    },
  ]);

  return {
    notificationContentTypeMap,
    notificationContentTypeOptions,
    notificationLifetimeMap,
    notificationLifetimeOptions,
    notificationSeverityOptions,
    notificationTypeMap,
    notificationTypeOptions,
  };
}

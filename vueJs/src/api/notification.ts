import VueI18n from 'vue-i18n'
import { urlStringify } from '@/utils'

import ApiService from './serviceBase'
import { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class NotificationApiService {
  public static getNotifications(payload: UserNotificationGetByPaged) {
    let _url = '/api/my-notifilers?'
    _url += urlStringify(payload)
    return ApiService.Get<PagedResultDto<NotificationInfo>>(_url, serviceUrl)
  }

  public static getAssignableNotifiers() {
    const _url = '/api/my-notifilers/assignables'
    return ApiService.Get<ListResultDto<NotificationGroup>>(_url, serviceUrl)
  }

  public static getMySubscribedNotifiers() {
    const _url = '/api/my-subscribes/all'
    return ApiService.Get<ListResultDto<UserSubscreNotification>>(_url, serviceUrl)
  }

  public static subscribeNotifier(payload: UserSubscreNotification) {
    const _url = '/api/my-subscribes'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  public static unSubscribeNotifier(name: string) {
    const _url = '/api/my-subscribes?name=' + name
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum NotificationReadState {
  Read = 0,
  UnRead = 1
}

export enum NotificationLifetime {
  Persistent = 0,
  OnlyOne = 1
}

export enum NotificationType {
  Application = 0,
  System = 10,
  User = 20
}

export enum NotificationSeverity {
  Success = 0,
  Info = 10,
  Warn = 20,
  Error = 30,
  Fatal = 40
}

export class NotificationData {
  properties: {[key: string]: any} = {}
}

export class NotificationInfo {
  tenantId? = ''
  name = ''
  id = ''
  data!: NotificationData
  creationTime!: Date
  lifetime!: NotificationLifetime
  type!: NotificationType
  severity!: NotificationSeverity

  public static tryParseNotifier(notifier: NotificationInfo, l: VueI18n) {
    const data = new NotificationData()
    data.properties = notifier.data.properties
    if (notifier.data.properties.localizer) {
      data.properties.title = l.t(data.properties.title.resourceName + '.' + data.properties.title.name, data.properties.title.values)
      data.properties.message = l.t(data.properties.message.resourceName + '.' + data.properties.message.name, data.properties.message.values)
      if (data.properties.description) {
        data.properties.description = l.t(data.properties.description.resourceName + '.' + data.properties.description.name, data.properties.description.values)
      }
      notifier.data = data
    }
    return notifier
  }
}

export class UserNotificationGetByPaged extends PagedAndSortedResultRequestDto {
  skipCount = 0
  maxResultCount = 10
  filter = ''
  reverse = true
  readState = NotificationReadState.UnRead
}

export class Notification {
  name = ''
  displayName = ''
  description = ''
  lifetime = NotificationLifetime.Persistent
  type = NotificationType.User
}

export class NotificationGroup {
  name = ''
  displayName = ''
  notifications = new Array<Notification>()
}

export class UserSubscreNotification {
  name = ''
}

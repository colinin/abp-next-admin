<template>
  <div
    v-infinite-scroll="onNotifiersScrollChanged"
    class="notification"
    style="max-height: 200px;overflow-y: auto;"
  >
    <List
      size="small"
      item-layout="vertical"
    >
      <ListItem
        v-for="(notify) in notifications"
        :key="notify.id"
      >
        <ListItemMeta
          :title="notify.message"
          :description="formatDateTime(notify.datetime)"
          style="cursor:pointer"
          @click.native="handleClickNotification(notify.id)"
        >
          <template slot="avatar">
            <Avatar
              icon="ios-person"
            />
          </template>
        </ListItemMeta>
      </ListItem>
    </List>
    {{ notifications.length === 0 ? $t('messages.noNotifications') : '' }}
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { UserModule } from '@/store/modules/user'
import { dateFormat, abpPagerFormat } from '@/utils/index'

import { MessageType } from 'element-ui/types/message'

import NotificationApiService, {
  NotificationInfo,
  NotificationSeverity as Severity,
  NotificationReadState as ReadState,
  UserNotificationGetByPaged
} from '@/api/notification'

class Notification {
  id!: string
  title!: string
  message!: string
  datetime!: Date
  severity!: Severity
}

@Component({
  name: 'UserNofitications'
})
export default class extends mixins(EventBusMiXin) {
  private connection!: HubConnection
  private notifications = new Array<Notification>()
  private allowRefreshNotifier = false
  private getMyNotifierFilter = new UserNotificationGetByPaged()

  mounted() {
    this.handleStartConnection()
  }

  destroyed() {
    this.handleStopConnection()
  }

  private renderIconType(item: any) {
    if (item.severity !== Severity.Success) {
      return ' el-icon-circle-close'
    }
    return ' el-icon-circle-check'
  }

  private renderIconStyle(item: any) {
    if (item.severity !== Severity.Success) {
      return 'backgroundColor: #f56a00'
    }
    return 'backgroundColor: #87d068'
  }

  private formatDateTime(datetime: string) {
    const date = new Date(datetime)
    return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
  }

  private handleStartConnection() {
    console.log('start signalr connection...')
    if (!this.connection) {
      const builder = new HubConnectionBuilder()
      const userToken = UserModule.token.replace('Bearer ', '')
      this.connection = builder
        .withUrl('/signalr-hubs/signalr-hubs/notifications', { accessTokenFactory: () => userToken })
        .withAutomaticReconnect({ nextRetryDelayInMilliseconds: () => 60000 })
        .build()
      this.connection.on('getNotification', data => this.onNotificationReceived(data))
      this.connection.onclose(error => {
        console.log('signalr connection has closed, error:')
        console.log(error)
      })
    }
    if (this.connection.state !== HubConnectionState.Connected) {
      this.connection.start()
        .then(() => {
          NotificationApiService
            .getNotifications(this.getMyNotifierFilter)
            .then(res => {
              res.items.forEach((notify: NotificationInfo) => {
                const notifier = NotificationInfo.tryParseNotifier(notify, this.$i18n)
                const notification = new Notification()
                notification.id = notifier.id
                notification.title = notifier.data.properties.title
                notification.message = notifier.data.properties.message
                notification.datetime = notifier.creationTime
                notification.severity = notifier.severity
                this.notifications.push(notification)
                this.trigger('onNotificationReceived', notify)
              })
            })
        })
    }
  }

  private handleStopConnection() {
    console.log('stop signalr connection...')
    if (this.connection && this.connection.state === HubConnectionState.Connected) {
      this.connection.stop()
    }
  }

  private onNotifiersScrollChanged() {
    console.log('onNotifiersScrollChanged')
    if (this.allowRefreshNotifier) {
      this.allowRefreshNotifier = false
      this.getMyNotifierFilter.skipCount = abpPagerFormat(this.getMyNotifierFilter.skipCount, this.getMyNotifierFilter.maxResultCount)
      NotificationApiService
        .getNotifications(this.getMyNotifierFilter)
        .then(res => {
          res.items.forEach((notify: NotificationInfo) => {
            const notifier = NotificationInfo.tryParseNotifier(notify, this.$i18n)
            const notification = new Notification()
            notification.id = notifier.id
            notification.title = notifier.data.properties.title
            notification.message = notifier.data.properties.message
            notification.datetime = notifier.creationTime
            notification.severity = notifier.severity
            this.notifications.push(notification)
            this.trigger('onNotificationReceived', notify)
          })
          this.getMyNotifierFilter.skipCount += 1
          if (res.items.length === 0) {
            this.allowRefreshNotifier = true
          }
        })
    }
  }

  private onNotificationReceived(notify: NotificationInfo) {
    const notifier = NotificationInfo.tryParseNotifier(notify, this.$i18n)
    const notification = new Notification()
    notification.id = notifier.id
    notification.title = notifier.data.properties.title
    notification.message = notifier.data.properties.message
    notification.datetime = notifier.creationTime
    notification.severity = notifier.severity
    this.pushUserNotification(notification)
    this.trigger(notifier.name, notify)
    this.trigger('onNotificationReceived', notify)
    this.$notify({
      title: notification.title,
      message: notification.message,
      type: this.getNofiyType(notification.severity)
    })
  }

  private handleClickNotification(notificationId: string) {
    this.connection.invoke('ChangeState', notificationId, ReadState.Read).then(() => {
      const removeNotifyIndex = this.notifications.findIndex(n => n.id === notificationId)
      this.notifications.splice(removeNotifyIndex, 1)
      this.trigger('onNotificationReadChanged')
    })
  }

  private pushUserNotification(notification: any) {
    if (this.notifications.length === 20) {
      this.notifications.shift()
    }
    this.notifications.push(notification)
  }

  private getNofiyType(severity: Severity) {
    const mapNotifyType: {[key: number]: MessageType } = {
      0: 'success',
      10: 'info',
      20: 'warning',
      30: 'error',
      40: 'error'
    }
    return mapNotifyType[severity]
  }
}
</script>

<style lang="scss">
.item {
  margin-top: 0px;
  margin-right: 10px;
}
.item.el-dropdown-selfdefine > .el-badge__content.el-badge__content--undefined.is-fixed {
  top: 10px;
}
.notification > .ivu-list.ivu-list-small.ivu-list-horizontal.ivu-list-split{
  max-height: 200px;
  overflow: auto;
}
</style>

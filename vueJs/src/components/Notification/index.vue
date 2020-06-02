<template>
  <el-dropdown trigger="click">
    <el-badge
      :value="notifications.length"
      class="item"
      :hidden="notifications.length<=0"
    >
      <svg-icon
        name="message"
        class="message-icon"
      />
    </el-badge>
    <el-dropdown-menu slot="dropdown">
      <div
        class="app-container"
        style="width: 400px;max-height: 300px;"
      >
        <el-tabs
          stretch
        >
          <el-tab-pane
            label="通知"
            class="notification"
          >
            <List
              size="small"
            >
              <ListItem
                v-for="(notify) in notifications"
                :key="notify.id"
              >
                <ListItemMeta
                  :title="notify.message"
                  :description="formatDateTime(notify.datetime)"
                  @click="handleClickNotification(notify.id)"
                >
                  <template slot="avatar">
                    <Avatar
                      icon="ios-person"
                    />
                  </template>
                </ListItemMeta>
              </ListItem>
            </List>
          </el-tab-pane>
          <el-tab-pane label="消息">
            消息系统
          </el-tab-pane>
        </el-tabs>
      </div>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script lang="ts">
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { Component, Vue } from 'vue-property-decorator'
import { UserModule } from '@/store/modules/user'
import { dateFormat } from '@/utils/index'
import { MessageType } from 'element-ui/types/message'

enum Severity {
  success = 0,
  info = 10,
  warn = 20,
  error = 30,
  fatal = 40
}

enum ReadState {
  Read = 0,
  UnRead = 1
}

class Notification {
  id!: string
  title!: string
  message!: string
  datetime!: Date
  severity!: Severity
}

@Component({
  name: 'Notification'
})
export default class extends Vue {
  private connection!: HubConnection
  private notifications = new Array<Notification>()

  mounted() {
    this.handleStartConnection()
  }

  destroyed() {
    this.handleStopConnection()
  }

  private renderIconType(item: any) {
    if (item.severity !== Severity.success) {
      return ' el-icon-circle-close'
    }
    return ' el-icon-circle-check'
  }

  private renderIconStyle(item: any) {
    if (item.severity !== Severity.success) {
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
      this.connection.start().then(() => {
        this.connection.invoke('GetNotification', ReadState.UnRead, 10).then(result => {
          console.log(result)
          result.items.forEach((notify: any) => {
            const notification = notify.data.properties
            notification.id = notify.id
            this.notifications.push(notification)
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

  private onNotificationReceived(data: any) {
    console.log('received signalr message...')
    console.log(data)
    const notification = data.data.properties
    this.pushUserNotification(notification)
    this.$notify({
      title: notification.title,
      message: notification.message,
      type: this.getNofiyType(data.notificationSeverity)
    })
  }

  private handleClickNotification(notificationId: string) {
    console.log('handleClickNotification')
    this.connection.invoke('ChangeState', notificationId, ReadState.Read).then(() => {
      const removeNotifyIndex = this.notifications.findIndex(n => n.id === notificationId)
      this.notifications.splice(removeNotifyIndex)
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
